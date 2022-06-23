using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using kiosk_solution.Business.Utilities;
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Repositories;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace kiosk_solution.Business.Services.impl
{
    public class PoiService : IPoiService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IPoiService> _logger;
        private readonly IMapService _mapService;
        private readonly IImageService _imageService;
        private readonly IKioskService _kioskService;

        public PoiService(IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<IPoiService> logger, IMapService mapService, IImageService imageService,
            IKioskService kioskService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapService = mapService;
            _imageService = imageService;
            _kioskService = kioskService;
        }

        public async Task<PoiImageViewModel> AddImageToPoi(Guid partyId, string roleName, PoiAddImageViewModel model)
        {
            List<ImageViewModel> listPoiImage = new List<ImageViewModel>();
            var poi = await _unitOfWork.PoiRepository
                .Get(p => p.Id.Equals(model.Id))
                .FirstOrDefaultAsync();

            if (poi == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found");
            }

            if (poi.Type.Equals(TypeConstants.CREATE_BY_ADMIN) && !roleName.Equals(RoleConstants.ADMIN))
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "You can not use this feature.");
            }

            if (poi.Type.Equals(TypeConstants.CREATE_BY_LOCATION_OWNER) && !poi.CreatorId.Equals(partyId))
            {
                _logger.LogInformation("You can not interact with poi which is not your.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest,
                    "You can not interact with poi which is not your.");
            }

            foreach (var img in model.ListImage)
            {
                ImageCreateViewModel imageModel = new ImageCreateViewModel(poi.Name, img,
                    poi.Id, CommonConstants.POI_IMAGE, CommonConstants.SOURCE_IMAGE);
                var image = await _imageService.Create(imageModel);
                listPoiImage.Add(image);
            }

            var poiImage = _mapper.Map<List<PoiImageDetailViewModel>>(listPoiImage);
            var result = await _unitOfWork.PoiRepository
                .Get(e => e.Id.Equals(model.Id))
                .ProjectTo<PoiImageViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            result.ListImage = poiImage;
            return result;
        }

        public async Task<PoiViewModel> Create(Guid partyId, string roleName, PoiCreateViewModel model)
        {
            List<ImageViewModel> listPoiImage = new List<ImageViewModel>();
            var poi = _mapper.Map<Poi>(model);
            var address = $"{poi.Address}, {poi.Ward}, {poi.District}, {poi.City}";
            var geoCodeing = await _mapService.GetForwardGeocode(address);
            poi.Longtitude = geoCodeing.GeoMetries[0].Lng;
            poi.Latitude = geoCodeing.GeoMetries[0].Lat;
            poi.OpenTime = TimeSpan.Parse(model.StringOpenTime);
            poi.CloseTime = TimeSpan.Parse(model.StringCloseTime);
            poi.CreateDate = DateTime.Now;
            poi.CreatorId = partyId;
            poi.Status = StatusConstants.ACTIVE;
            if (roleName.Equals(RoleConstants.ADMIN))
                poi.Type = TypeConstants.CREATE_BY_ADMIN;
            else
                poi.Type = TypeConstants.CREATE_BY_LOCATION_OWNER;
            try
            {
                await _unitOfWork.PoiRepository.InsertAsync(poi);
                await _unitOfWork.SaveAsync();

                ImageCreateViewModel thumbnailModel = new ImageCreateViewModel(poi.Name,
                    model.Thumbnail, poi.Id, CommonConstants.POI_IMAGE, CommonConstants.THUMBNAIL);

                var thumbnail = await _imageService.Create(thumbnailModel);

                var result = await _unitOfWork.PoiRepository
                    .Get(p => p.Id.Equals(poi.Id))
                    .Include(p => p.Creator)
                    .Include(p => p.Poicategory)
                    .ProjectTo<PoiViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                foreach (var img in model.ListImage)
                {
                    ImageCreateViewModel imageModel = new ImageCreateViewModel(result.Name, img,
                    result.Id, CommonConstants.POI_IMAGE, CommonConstants.SOURCE_IMAGE);
                    var image = await _imageService.Create(imageModel);
                    listPoiImage.Add(image);
                }

                var poiImage = _mapper.Map<List<PoiImageDetailViewModel>>(listPoiImage);
                result.Thumbnail = thumbnail;
                result.ListImage = poiImage;
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<PoiViewModel> UpdateInformation(Guid partyId, string roleName, PoiInfomationUpdateViewModel model)
        {
            var poi = await _unitOfWork.PoiRepository
                .Get(p => p.Id.Equals(model.Id))
                .FirstOrDefaultAsync();
            if (poi == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }

            if (poi.Type.Equals(TypeConstants.CREATE_BY_ADMIN) && !roleName.Equals(RoleConstants.ADMIN))
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "You can not use this feature.");
            }

            if (poi.Type.Equals(TypeConstants.CREATE_BY_LOCATION_OWNER) && !poi.CreatorId.Equals(partyId))
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "You can not use this feature.");
            }

            poi.Name = model.Name;
            poi.Description = model.Description;
            var address = $"{model.Address}, {model.Ward}, {model.District}, {model.City}";
            var geoCodeing = await _mapService.GetForwardGeocode(address);
            poi.Longtitude = geoCodeing.GeoMetries[0].Lng;
            poi.Latitude = geoCodeing.GeoMetries[0].Lat;
            poi.OpenTime = TimeSpan.Parse(model.StringOpenTime);
            poi.CloseTime = TimeSpan.Parse(model.StringCloseTime);
            poi.PoicategoryId = model.PoicategoryId;
            poi.DayOfWeek = model.DayOfWeek;
            try
            {
                 _unitOfWork.PoiRepository.Update(poi);
                 await _unitOfWork.SaveAsync();
                 var result = _mapper.Map<PoiViewModel>(poi);
                 return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<PoiViewModel> DeleteImageFromPoi(Guid partyId, string roleName, Guid imageId)
        {
            var image = await _imageService.GetById(imageId);
            if (!image.KeyType.Equals(CommonConstants.POI_IMAGE))
            {
                _logger.LogInformation("You can not delete event image.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "You can not delete event image.");
            }
            var poi = await _unitOfWork.PoiRepository
                .Get(p => p.Id.Equals(image.KeyId))
                .ProjectTo<PoiViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (poi == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }

            if (poi.Type.Equals(TypeConstants.CREATE_BY_ADMIN) && !roleName.Equals(RoleConstants.ADMIN))
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "You can not use this feature.");
            }

            if (poi.Type.Equals(TypeConstants.CREATE_BY_LOCATION_OWNER) && !poi.CreatorId.Equals(partyId))
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "You can not use this feature.");
            }
            if (image.Link.Contains(CommonConstants.THUMBNAIL))
            {
                _logger.LogInformation("You can not delete thumbnail. You can only change thumbnail.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "You can not delete thumbnail. You can only change thumbnail.");
            }
            bool delete = await _imageService.Delete(imageId);
            if (delete)
            {
                var listImage = await _imageService.GetByKeyIdAndKeyType(Guid.Parse(poi.Id + ""), CommonConstants.POI_IMAGE);
                if (listImage == null)
                {
                    _logger.LogInformation($"{poi.Name} has lost image.");
                    throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
                }
                var listSourceImage = new List<ImageViewModel>();
                foreach (var img in listImage)
                {
                    if (img.Link == null)
                    {
                        _logger.LogInformation($"{poi.Name} has lost image.");
                        throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
                    }
                    if (img.Link.Contains(CommonConstants.THUMBNAIL))
                    {
                        poi.Thumbnail = img;
                    }
                    else if (img.Link.Contains(CommonConstants.SOURCE_IMAGE))
                    {
                        listSourceImage.Add(img);
                    }
                }
                poi.ListImage = _mapper.Map<List<PoiImageDetailViewModel>>(listSourceImage);
                return poi;
            }
            else
            {
                _logger.LogInformation("Server Error.");
                throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Server Error.");
            }
        }

        public async Task<DynamicModelResponse<PoiSearchViewModel>> GetAllWithPaging(Guid partyId, string role, PoiSearchViewModel model, int size, int pageNum)
        {
            IQueryable<PoiSearchViewModel> pois = null;
            if(role.Equals(RoleConstants.ADMIN))
            {
                pois = _unitOfWork.PoiRepository.Get()
                    .ProjectTo<PoiSearchViewModel>(_mapper.ConfigurationProvider)
                    .DynamicFilter(model);
            }
            else if(role.Equals(RoleConstants.LOCATION_OWNER))
            {
                pois = _unitOfWork.PoiRepository.Get(p => p.CreatorId.Equals(partyId))
                    .ProjectTo<PoiSearchViewModel>(_mapper.ConfigurationProvider)
                    .DynamicFilter(model);
            }

            var listPoi = pois.ToList();
            
            foreach(var item in listPoi)
            {
                var listImage = await _imageService
                    .GetByKeyIdAndKeyType(Guid.Parse(item.Id + ""), CommonConstants.POI_IMAGE);
                if(listImage == null)
                {
                    _logger.LogInformation($"{item.Name} has lost image.");
                    throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
                }
                var listSourceImage = new List<ImageViewModel>();
                foreach(var img in listImage)
                {
                    if(img.Link == null)
                    {
                        _logger.LogInformation($"{item.Name} has lost image.");
                        throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
                    }
                    if (img.Link.Contains(CommonConstants.THUMBNAIL))
                    {
                        item.Thumbnail = img;
                    }
                    else if (img.Link.Contains(CommonConstants.SOURCE_IMAGE))
                    {
                        listSourceImage.Add(img);
                    }
                }
                item.ListImage = _mapper.Map<List<PoiImageDetailViewModel>>(listSourceImage);
            }

            pois = listPoi.AsQueryable().OrderByDescending(p => p.Name);

            var listPaging =
                pois.PagingIQueryable(pageNum, size, CommonConstants.LimitPaging, CommonConstants.DefaultPaging);

            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found");
            }

            var result = new DynamicModelResponse<PoiSearchViewModel>
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

        public async Task<PoiSearchViewModel> GetById(Guid id)
        {
            var item = _unitOfWork.PoiRepository
                .Get(poi => poi.Id.Equals(id))
                .Include(poi => poi.Poicategory)
                .ProjectTo<PoiSearchViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault();
            var listImage = await _imageService
                .GetByKeyIdAndKeyType(Guid.Parse(item.Id + ""), CommonConstants.POI_IMAGE);
            if(listImage == null)
            {
                _logger.LogInformation($"{item.Name} has lost image.");
                throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
            }
            var listSourceImage = new List<ImageViewModel>();
            foreach(var img in listImage)
            {
                if(img.Link == null)
                {
                    _logger.LogInformation($"{item.Name} has lost image.");
                    throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
                }
                if (img.Link.Contains(CommonConstants.THUMBNAIL))
                {
                    item.Thumbnail = img;
                }
                else if (img.Link.Contains(CommonConstants.SOURCE_IMAGE))
                {
                    listSourceImage.Add(img);
                }
            }
            item.ListImage = _mapper.Map<List<PoiImageDetailViewModel>>(listSourceImage);
            return item;
        }

        public async Task<List<PoiViewModel>> GetLocationNearby(Guid kioskId, double lng, double lat)
        {
            var kiosk = await _kioskService.GetById(kioskId);
            var pois = _unitOfWork.PoiRepository
                .GetPoiNearBy(Guid.Parse(kiosk.PartyId+""), lng, lat)
                .ProjectTo<PoiViewModel>(_mapper.ConfigurationProvider);
            var listPoi = pois.ToList();
            if (listPoi.Count() < 1)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");

            }
            foreach (var item in listPoi)
            {
                var listImage = await _imageService
                    .GetByKeyIdAndKeyType(Guid.Parse(item.Id + ""), CommonConstants.POI_IMAGE);
                if (listImage == null)
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
                    if (img.Link.Contains(CommonConstants.THUMBNAIL))
                    {
                        item.Thumbnail = img;
                    }
                    else if (img.Link.Contains(CommonConstants.SOURCE_IMAGE))
                    {
                        listSourceImage.Add(img);
                    }
                }
                item.ListImage = _mapper.Map<List<PoiImageDetailViewModel>>(listSourceImage);
            }
            return listPoi;
        }

        public async Task<ImageViewModel> UpdateImageToPoi(Guid partyId, string roleName, PoiUpdateImageViewModel model)
        {
            var img = await _imageService.GetById(model.Id);

            if (!img.KeyType.Equals(CommonConstants.POI_IMAGE))
            {
                _logger.LogInformation("You can not update event image.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "You can not update event image.");
            }

            var poi = await _unitOfWork.PoiRepository
                .Get(p => p.Id.Equals(img.KeyId))
                .FirstOrDefaultAsync();

            if (poi == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found");
            }

            if (poi.Type.Equals(TypeConstants.CREATE_BY_ADMIN) && !roleName.Equals(RoleConstants.ADMIN))
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "You can not use this feature.");
            }

            if (poi.Type.Equals(TypeConstants.CREATE_BY_LOCATION_OWNER) && !poi.CreatorId.Equals(partyId))
            {
                _logger.LogInformation("You can not interact with poi which is not your.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest,
                    "You can not interact with poi which is not your.");
            }

            ImageUpdateViewModel updateModel =
                new ImageUpdateViewModel(img.Id, poi.Name, model.Image, CommonConstants.SOURCE_IMAGE);

            var result = await _imageService.Update(updateModel);
            return result;
        }
    }
}
