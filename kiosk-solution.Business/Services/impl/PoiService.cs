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

        public PoiService(IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<IPoiService> logger, IMapService mapService, IImageService imageService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapService = mapService;
            _imageService = imageService;
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
                ImageCreateViewModel imageModel = new ImageCreateViewModel(poi.Name, img.Image,
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
                    ImageCreateViewModel imageModel = new ImageCreateViewModel(result.Name, img.Image,
                        result.Id, CommonConstants.EVENT_IMAGE, CommonConstants.SOURCE_IMAGE);
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

        public async Task<DynamicModelResponse<PoiSearchViewModel>> GetWithPaging(PoiSearchViewModel model, int size,
            int pageNum)
        {
            IOrderedQueryable<PoiSearchViewModel> pois;
            if (string.IsNullOrEmpty(model.Type))
            {
                pois = _unitOfWork.PoiRepository.Get()
                    .ProjectTo<PoiSearchViewModel>(_mapper.ConfigurationProvider)
                    .DynamicFilter(model)
                    .AsQueryable()
                    .OrderByDescending(p => p.Name);
            }
            else
            {
                pois = _unitOfWork.PoiRepository.Get(p => p.Type.Equals(model.Type))
                    .ProjectTo<PoiSearchViewModel>(_mapper.ConfigurationProvider).DynamicFilter(model).AsQueryable()
                    .OrderByDescending(p => p.Name);
            }

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
            return _unitOfWork.PoiRepository
                .Get(poi => poi.Id.Equals(id))
                .Include(poi => poi.Poicategory)
                .ProjectTo<PoiSearchViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault();
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