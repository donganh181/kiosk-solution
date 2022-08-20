using AutoMapper;
using AutoMapper.QueryableExtensions;
using kiosk_solution.Business.Hubs;
using kiosk_solution.Business.Utilities;
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Repositories;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Services.impl
{
    public class KioskLocationService : IKioskLocationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<IKioskLocationService> _logger;
        private readonly IImageService _imageService;
        private readonly IHubContext<SystemEventHub> _eventHub;

        public KioskLocationService(IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<IKioskLocationService> logger, IImageService imageService,
            IHubContext<SystemEventHub> eventHub)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _imageService = imageService;
            _eventHub = eventHub;
        }

        public async Task<KioskLocationViewModel> CreateNew(Guid partyId, CreateKioskLocationViewModel model)
        {
            List<ImageViewModel> listLocationImage = new List<ImageViewModel>();

            var check = await _unitOfWork.KioskLocationRepository
                .Get(l => l.OwnerId.Equals(partyId) && l.Name.Equals(model.Name))
                .FirstOrDefaultAsync();

            if (check != null)
            {
                _logger.LogInformation("You has already created this location.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "You has already created this location.");
            }

            if(model.ListImage == null)
            {
                _logger.LogInformation("Must have at least 1 image.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Must have at least 1 image.");
            }

            var location = _mapper.Map<KioskLocation>(model);
            location.OwnerId = partyId;
            location.CreateDate = DateTime.Now;
            try
            {
                await _unitOfWork.KioskLocationRepository.InsertAsync(location);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.KioskLocationRepository
                    .Get(l => l.Id.Equals(location.Id))
                    .Include(a => a.Owner)
                    .ProjectTo<KioskLocationViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                foreach (var img in model.ListImage)
                {
                    ImageCreateViewModel imageModel = new ImageCreateViewModel(result.Name, img,
                        result.Id, CommonConstants.LOCATION_IMAGE, CommonConstants.SOURCE_IMAGE);
                    var image = await _imageService.Create(imageModel);
                    listLocationImage.Add(image);
                }
                result.ListImage = listLocationImage;
                return result;
                 
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid Data.");
            }
        }

        public async Task<DynamicModelResponse<KioskLocationSearchViewModel>> GetAllWithPaging(Guid partyId, KioskLocationSearchViewModel model, int size, int pageNum)
        {
            var listLocation = _unitOfWork.KioskLocationRepository
                .Get(l => l.OwnerId.Equals(partyId))
                .Include(a => a.Owner)
                .ProjectTo<KioskLocationSearchViewModel>(_mapper.ConfigurationProvider)
                .ToList();

            foreach(var item in listLocation)
            {
                var listImage = await _imageService.GetByKeyIdAndKeyType(Guid.Parse(item.Id + ""), CommonConstants.LOCATION_IMAGE);
                if(listImage == null)
                {
                    _logger.LogInformation($"{item.Name} has lost image.");
                    throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
                }
                var listSourceImage = new List<ImageViewModel>();
                foreach (var img in listImage)
                {
                    if (img.Link == null)
                    {
                        _logger.LogInformation($"{item.Name} has lost image.");
                        throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
                    }
                    listSourceImage.Add(img);
                }
                item.ListImage = listSourceImage;
            }
            var locations = listLocation.AsQueryable().OrderByDescending(l => l.Name);

            var listPaging = locations
                .DynamicFilter(model)
                .PagingIQueryable(pageNum, size, CommonConstants.LimitPaging,
                CommonConstants.DefaultPaging);
            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not Found");
            }

            var result = new DynamicModelResponse<KioskLocationSearchViewModel>
            {
                Metadata = new PagingMetaData
                {
                    Page = pageNum,
                    Size = size,
                    Total = listPaging.Total
                },
                Data = listPaging.Data.ToList()
            };
            return result;
        }

        public async Task<KioskLocationViewModel> GetById(Guid id, bool isNotDes)
        {
            var location = await _unitOfWork.KioskLocationRepository
                .Get(l => l.Id.Equals(id))
                .Include(a => a.Owner)
                .ProjectTo<KioskLocationViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (location == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }
            if (isNotDes)
            {
                location.Description = String.Empty;
            }
            var listImage = await _imageService.GetByKeyIdAndKeyType(Guid.Parse(location.Id + ""), CommonConstants.LOCATION_IMAGE);
            if (listImage == null)
            {
                _logger.LogInformation($"{location.Name} has lost image.");
                throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
            }
            var listSourceImage = new List<ImageViewModel>();
            foreach (var img in listImage)
            {
                if (img.Link == null)
                {
                    _logger.LogInformation($"{location.Name} has lost image.");
                    throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
                }
                listSourceImage.Add(img);
            }
            location.ListImage = listSourceImage;

            return location;
        }

        public async Task<KioskLocationViewModel> GetByIdAndChangeKioskView(Guid id)
        {
            var location = await _unitOfWork.KioskLocationRepository
                .Get(l => l.Id.Equals(id))
                .Include(a => a.Owner)
                .Include(b => b.Kiosks)
                .FirstOrDefaultAsync();

            if(location == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }

            var listKiosk = location.Kiosks.ToList();

            if (listKiosk.Count < 1)
            {
                _logger.LogInformation("Can not found kiosk.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found kiosk.");
            }
            var result = _mapper.Map<KioskLocationViewModel>(location);
            var jsonConvert = JsonConvert.SerializeObject(result);

            foreach (var kiosk in listKiosk)
            {
                await _eventHub.Clients.Group(kiosk.Id.ToString())
                    .SendAsync(SystemEventHub.RELOAD_KIOSK_CHANNEL,
                        SystemEventHub.SYSTEM_BOT, jsonConvert);
                _logger.LogInformation($"Send model to kiosk id: {kiosk.Id} success.");
            }
            return result;
        }

        public async Task<KioskLocationViewModel> ReplaceImage(Guid partyId, ImageReplaceViewModel model)
        {
            var checkLocation = await _unitOfWork.KioskLocationRepository
                .Get(l => l.Id.Equals(model.Id) && l.OwnerId.Equals(partyId))
                .Include(a => a.Owner)
                .ProjectTo<KioskLocationViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if(checkLocation == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }
            if(model.RemoveFields != null)
            {
                foreach (var imageId in model.RemoveFields)
                {
                    var img = await _imageService.GetById(imageId);
                    if (!img.KeyType.Equals(CommonConstants.LOCATION_IMAGE))
                    {
                        _logger.LogInformation("You can not delete other type image.");
                        throw new ErrorResponse((int)HttpStatusCode.BadRequest, "You can not delete other type image.");
                    }
                    var myLocation = await _unitOfWork.KioskLocationRepository
                        .Get(l => l.Id.Equals(img.KeyId))
                        .ProjectTo<KioskLocationViewModel>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();
                    if (myLocation == null)
                    {
                        _logger.LogInformation("Can not found.");
                        throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
                    }
                    if (!myLocation.OwnerId.Equals(partyId))
                    {
                        _logger.LogInformation("You can not delete image of other user.");
                        throw new ErrorResponse((int)HttpStatusCode.BadRequest, "You can not delete image of other user.");
                    }
                    bool delete = await _imageService.Delete(imageId);
                    if (!delete)
                    {
                        _logger.LogInformation("Server Error.");
                        throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Server Error.");
                    }
                }
            }
            if(model.AddFields != null)
            {
                foreach (var image in model.AddFields)
                {
                    ImageCreateViewModel imageModel = new ImageCreateViewModel(checkLocation.Name, image,
                        checkLocation.Id, CommonConstants.LOCATION_IMAGE, CommonConstants.SOURCE_IMAGE);
                    var newImage = await _imageService.Create(imageModel);
                }
            }
            var listImage = await _imageService.GetByKeyIdAndKeyType(Guid.Parse(checkLocation.Id + ""), CommonConstants.LOCATION_IMAGE);
            if (listImage == null)
            {
                _logger.LogInformation($"{checkLocation.Name} has lost image.");
                throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
            }
            var listSourceImage = new List<ImageViewModel>();
            foreach (var img in listImage)
            {
                if (img.Link == null)
                {
                    _logger.LogInformation($"{checkLocation.Name} has lost image.");
                    throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
                }
                listSourceImage.Add(img);
            }
            checkLocation.ListImage = listSourceImage;
            return checkLocation;
        }

        public async Task<KioskLocationViewModel> UpdateInformation(Guid partyId, UpdateKioskLocationViewModel model)
        {
            var location = await _unitOfWork.KioskLocationRepository
                .Get(l => l.Id.Equals(model.Id))
                .FirstOrDefaultAsync();

            if (location == null)
            {
                _logger.LogInformation("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }
            if (!location.OwnerId.Equals(partyId))
            {
                _logger.LogInformation("You cannot update kiosk location of another user.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "You cannot update kiosk location of another user.");
            }
            location.Name = model.Name;
            location.Description = model.Description;
            location.HotLine = model.HotLine;

            try
            {
                _unitOfWork.KioskLocationRepository.Update(location);
                await _unitOfWork.SaveAsync();
                var result = await _unitOfWork.KioskLocationRepository
                    .Get(l => l.Id.Equals(location.Id))
                    .Include(a => a.Owner)
                    .ProjectTo<KioskLocationViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                var listImage = await _imageService.GetByKeyIdAndKeyType(Guid.Parse(result.Id + ""), CommonConstants.LOCATION_IMAGE);
                if(listImage == null)
                {
                    _logger.LogInformation($"{result.Name} has lost image.");
                    throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
                }
                var listSourceImage = new List<ImageViewModel>();
                foreach(var img in listImage)
                {
                    if (img.Link == null)
                    {
                        _logger.LogInformation($"{result.Name} has lost image.");
                        throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
                    }
                    listSourceImage.Add(img);
                }
                result.ListImage = listSourceImage;
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid Data.");
            }
        }
    }
}
