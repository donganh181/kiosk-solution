using AutoMapper;
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Repositories;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using kiosk_solution.Business.Utilities;
using Microsoft.EntityFrameworkCore;

namespace kiosk_solution.Business.Services.impl
{
    public class EventService : IEventService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IEventService> _logger;
        private readonly IImageService _imageService;
        private readonly IMapService _mapService;

        public EventService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<IEventService> logger, IImageService imageService, IMapService mapService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _imageService = imageService;
            _mapService = mapService;
        }

        public async Task<EventImageViewModel> AddImageToEvent(Guid partyId, string roleName, EventAddImageViewModel model)
        {
            List<ImageViewModel> listEventImage = new List<ImageViewModel>();
            var myEvent = await _unitOfWork.EventRepository.Get(e => e.Id.Equals(model.Id)).FirstOrDefaultAsync();

            if (myEvent == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }

            if (myEvent.Type.Equals(TypeConstants.SERVER_TYPE) && !roleName.Equals(RoleConstants.ADMIN))
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "You can not use this feature.");
            }

            if (myEvent.Type.Equals(TypeConstants.LOCAL_TYPE) && !myEvent.CreatorId.Equals(partyId))
            {
                _logger.LogInformation("You can not interact with event which is not your.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "You can not interact with event which is not your.");
            }

            foreach(var img in model.ListImage)
            {
                ImageCreateViewModel imageModel = new ImageCreateViewModel(myEvent.Name, img,
                    myEvent.Id, CommonConstants.EVENT_IMAGE, CommonConstants.SOURCE_IMAGE);
                var image = await _imageService.Create(imageModel);
                listEventImage.Add(image);
            }

            var eventImage = _mapper.Map<List<EventImageDetailViewModel>>(listEventImage);
            var result = await _unitOfWork.EventRepository
                .Get(e => e.Id.Equals(model.Id))
                .ProjectTo<EventImageViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            result.ListImage = eventImage;
            return result;
        }

        public async Task<EventViewModel> Create(Guid creatorId, string role, EventCreateViewModel model)
        {
            List<ImageViewModel> listEventImage = new List<ImageViewModel>();
            var newEvent = _mapper.Map<Event>(model);

            newEvent.CreatorId = creatorId;

            var TimeStart = DateTime.Parse(newEvent.TimeStart + "");
            var TimeEnd = DateTime.Parse(newEvent.TimeEnd + "");

            //case time start = time end or time start > time end
            if (DateTime.Compare(TimeStart, TimeEnd) == 0 || DateTime.Compare(TimeStart, TimeEnd) > 0)
            {
                _logger.LogInformation("Time start cannot happen at the same time or later than time end.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest,
                    "Time start cannot happen at the same time or later than time end.");
            }

            ///case now > time end
            if (DateTime.Compare(DateTime.Now, TimeEnd) > 0)
            {
                _logger.LogInformation("This event is end.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "This event is end.");
            }

            //case time now < start
            if (DateTime.Compare(TimeStart, DateTime.Now) > 0)
            {
                newEvent.Status = StatusConstants.COMING_SOON;
            }
            //case time start <  now  < time end
            else if (DateTime.Compare(TimeStart, DateTime.Now) < 0 && DateTime.Compare(DateTime.Now, TimeEnd) < 0)
            {
                newEvent.Status = StatusConstants.ON_GOING;
            }

            if (role.Equals(RoleConstants.ADMIN))
            {
                newEvent.Type = TypeConstants.SERVER_TYPE;
            }
            else if (role.Equals(RoleConstants.LOCATION_OWNER))
            {
                newEvent.Type = TypeConstants.LOCAL_TYPE;
            }
            else
            {
                _logger.LogInformation("Server Error.");
                throw new ErrorResponse((int) HttpStatusCode.InternalServerError, "Server Error.");
            }
            var address = $"{newEvent.Address}, {newEvent.Ward}, {newEvent.District}, {newEvent.City}";
            var geoCodeing = await _mapService.GetForwardGeocode(address);
            newEvent.Longtitude = geoCodeing.GeoMetries[0].Lng;
            newEvent.Latitude = geoCodeing.GeoMetries[0].Lat;
            newEvent.CreateDate = DateTime.Now;
            try
            {                
                await _unitOfWork.EventRepository.InsertAsync(newEvent);
                await _unitOfWork.SaveAsync();

                ImageCreateViewModel thumbnailModel = new ImageCreateViewModel(newEvent.Name,
                    model.Thumbnail, newEvent.Id, CommonConstants.EVENT_IMAGE, CommonConstants.THUMBNAIL);

                var thumbnail = await _imageService.Create(thumbnailModel);
                
                var result = await _unitOfWork.EventRepository
                    .Get(e => e.Id.Equals(newEvent.Id))
                    .Include(e => e.Creator)
                    .ProjectTo<EventViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                foreach (var img in model.ListImage)
                {
                    ImageCreateViewModel imageModel = new ImageCreateViewModel(result.Name, img,
                    result.Id, CommonConstants.EVENT_IMAGE, CommonConstants.SOURCE_IMAGE);
                    var image = await _imageService.Create(imageModel);
                    listEventImage.Add(image);
                }
                var eventImage = _mapper.Map<List<EventImageDetailViewModel>>(listEventImage);

                result.Thumbnail = thumbnail;
                result.ListImage = eventImage;
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid Data.");
            }
        }

        public async Task<EventViewModel> DeleteImageFromEvent(Guid partyId, string roleName, Guid imageId)
        {
            var image = await _imageService.GetById(imageId);
            if (!image.KeyType.Equals(CommonConstants.EVENT_IMAGE))
            {
                _logger.LogInformation("You can not delete poi image.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "You can not delete poi image.");
            }
            var myEvent = await _unitOfWork.EventRepository
                .Get(e => e.Id.Equals(image.KeyId))
                .ProjectTo<EventViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (myEvent == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }

            if (myEvent.Type.Equals(TypeConstants.SERVER_TYPE) && !roleName.Equals(RoleConstants.ADMIN))
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "You can not use this feature.");
            }

            if (myEvent.Type.Equals(TypeConstants.LOCAL_TYPE) && !myEvent.CreatorId.Equals(partyId))
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
                var listImage = await _imageService.GetByKeyIdAndKeyType(Guid.Parse(myEvent.Id + ""), CommonConstants.EVENT_IMAGE);
                if (listImage == null)
                {
                    _logger.LogInformation($"{myEvent.Name} has lost image.");
                    throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
                }
                var listSourceImage = new List<ImageViewModel>();
                foreach (var img in listImage)
                {
                    if (img.Link == null)
                    {
                        _logger.LogInformation($"{myEvent.Name} has lost image.");
                        throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
                    }
                    if (img.Link.Contains(CommonConstants.THUMBNAIL))
                    {
                        myEvent.Thumbnail = img;
                    }
                    else if (img.Link.Contains(CommonConstants.SOURCE_IMAGE))
                    {
                        listSourceImage.Add(img);
                    }
                }
                myEvent.ListImage = _mapper.Map<List<EventImageDetailViewModel>>(listSourceImage);
                return myEvent;
            }
            else
            {
                _logger.LogInformation("Server Error.");
                throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Server Error.");
            }
        }

        public async Task<EventViewModel> Delete(Guid partyId, string roleName, Guid eventId)
        {
            var evt = await _unitOfWork.EventRepository.Get(e => e.Id.Equals(eventId)).FirstOrDefaultAsync();
            if (evt == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }

            if (evt.Type.Equals(TypeConstants.SERVER_TYPE) && !roleName.Equals(RoleConstants.ADMIN))
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "You can not use this feature.");
            }

            if (evt.Type.Equals(TypeConstants.LOCAL_TYPE) && !evt.CreatorId.Equals(partyId))
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "You can not use this feature.");
            }

            try
            {
                _unitOfWork.EventRepository.Delete(evt);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<EventViewModel>(evt);
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<DynamicModelResponse<EventSearchViewModel>> GetAllWithPaging(Guid? partyId, string roleName,
            EventSearchViewModel model, int size, int pageNum)
        {
            IQueryable<EventSearchViewModel> events = null;
            if (!string.IsNullOrEmpty(roleName) && roleName.Equals(RoleConstants.LOCATION_OWNER))
            {
                events = _unitOfWork.EventRepository
                    .Get(e => (e.CreatorId.Equals(partyId) &&
                               e.Type.Equals(TypeConstants.LOCAL_TYPE)) || e.Type.Equals(TypeConstants.SERVER_TYPE))
                    .Include(e => e.Creator)
                    .ProjectTo<EventSearchViewModel>(_mapper.ConfigurationProvider);
            }
            else if(string.IsNullOrEmpty(roleName) || roleName.Equals(RoleConstants.ADMIN))
            {
                events = _unitOfWork.EventRepository
                    .Get()
                    .Include(e => e.Creator)
                    .ProjectTo<EventSearchViewModel>(_mapper.ConfigurationProvider);
            }
            else if(!string.IsNullOrEmpty(roleName) && roleName.Equals(RoleConstants.SERVICE_PROVIDER))
            {
                _logger.LogInformation("Your role can not use this feature.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "Your role can not use this feature.");
            }

            var listEvent = events.ToList();

            foreach (var item in listEvent)
            {
                var listImage = await _imageService.GetByKeyIdAndKeyType(Guid.Parse(item.Id + ""), CommonConstants.EVENT_IMAGE);
                if (listImage == null)
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
                item.ListImage = _mapper.Map<List<EventImageDetailViewModel>>(listSourceImage);
            }

            events = listEvent.AsQueryable().OrderByDescending(e => e.CreateDate);

            var listPaging = events
                .DynamicFilter(model)
                .PagingIQueryable(pageNum, size, CommonConstants.LimitPaging,
                CommonConstants.DefaultPaging);
            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found");
            }

            var result = new DynamicModelResponse<EventSearchViewModel>
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

        public async Task<EventViewModel> GetById(Guid id)
        {
            var myEvent = await _unitOfWork.EventRepository
                .Get(e => e.Id.Equals(id))
                .ProjectTo<EventViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            var listImage = await _imageService.GetByKeyIdAndKeyType(id, CommonConstants.EVENT_IMAGE);
            if (listImage == null)
            {
                _logger.LogInformation($"{myEvent.Name} has lost image.");
                throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
            }
            var listSourceImage = new List<ImageViewModel>();
            foreach (var img in listImage)
            {
                if (img.Link == null)
                {
                    _logger.LogInformation($"{myEvent.Name} has lost image.");
                    throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
                }
                if (img.Link.Contains(CommonConstants.THUMBNAIL))
                {
                    myEvent.Thumbnail = img;
                }
                else if (img.Link.Contains(CommonConstants.SOURCE_IMAGE))
                {
                    listSourceImage.Add(img);
                }
            }
            myEvent.ListImage = _mapper.Map<List<EventImageDetailViewModel>>(listSourceImage);
            return myEvent;
        }

        public async Task<EventViewModel> Update(Guid partyId, EventUpdateViewModel model, string roleName)
        {
            var eventUpdate = await _unitOfWork.EventRepository
                .Get(e => e.Id.Equals(model.Id))
                .FirstOrDefaultAsync();
            if (eventUpdate == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not found.");
            }

            if (eventUpdate.Type.Equals(TypeConstants.SERVER_TYPE) && !roleName.Equals(RoleConstants.ADMIN))
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "You can not use this feature.");
            }
            if (eventUpdate.Type.Equals(TypeConstants.LOCAL_TYPE) && !eventUpdate.CreatorId.Equals(partyId))
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "You can not use this feature.");
            }
            var address = $"{eventUpdate.Address}, {eventUpdate.Ward}, {eventUpdate.District}, {eventUpdate.City}";
            var geoCodeing = await _mapService.GetForwardGeocode(address);
            eventUpdate.Longtitude = geoCodeing.GeoMetries[0].Lng;
            eventUpdate.Latitude = geoCodeing.GeoMetries[0].Lat;
            eventUpdate.Name = model.Name;
            eventUpdate.Description = model.Description;
            eventUpdate.Address = model.Address;
            eventUpdate.TimeStart = model.TimeStart;
            eventUpdate.TimeEnd = model.TimeEnd;
            eventUpdate.City = model.City;
            eventUpdate.District = model.District;
            eventUpdate.Ward = model.Ward;

            var TimeStart = DateTime.Parse(eventUpdate.TimeStart + "");
            var TimeEnd = DateTime.Parse(eventUpdate.TimeEnd + "");

            //case time start = time end or time start > time end
            if (DateTime.Compare(TimeStart, TimeEnd) == 0 || DateTime.Compare(TimeStart, TimeEnd) > 0)
            {
                _logger.LogInformation("Time start cannot happen at the same time or later than time end.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest,
                    "Time start cannot happen at the same time or later than time end.");
            }

            ///case now > time end
            if (DateTime.Compare(DateTime.Now, TimeEnd) > 0)
            {
                _logger.LogInformation("This event is end.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "This event is end.");
            }

            //case time now < start
            if (DateTime.Compare(TimeStart, DateTime.Now) > 0)
            {
                eventUpdate.Status = StatusConstants.COMING_SOON;
            }
            //case time start <  now  < time end
            else if (DateTime.Compare(TimeStart, DateTime.Now) < 0 && DateTime.Compare(DateTime.Now, TimeEnd) < 0)
            {
                eventUpdate.Status = StatusConstants.ON_GOING;
            }

            try
            {
                _unitOfWork.EventRepository.Update(eventUpdate);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<EventViewModel>(eventUpdate);
                if (model.ImageId == null || string.IsNullOrEmpty(model.Image))
                {
                    var listImage = await _imageService.GetByKeyIdAndKeyType(Guid.Parse(result.Id + ""), CommonConstants.EVENT_IMAGE);
                    if (listImage == null)
                    {
                        _logger.LogInformation($"{result.Name} has lost image.");
                        throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
                    }
                    var listSourceImage = new List<ImageViewModel>();
                    foreach (var img in listImage)
                    {
                        if (img.Link == null)
                        {
                            _logger.LogInformation($"{result.Name} has lost image.");
                            throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
                        }
                        if (img.Link.Contains(CommonConstants.THUMBNAIL))
                        {
                            result.Thumbnail = img;
                        }
                        else if (img.Link.Contains(CommonConstants.SOURCE_IMAGE))
                        {
                            listSourceImage.Add(img);
                        }
                    }
                    result.ListImage = _mapper.Map<List<EventImageDetailViewModel>>(listSourceImage);
                    return result;
                }
                else
                {
                    ImageUpdateViewModel imageUpdateModel = new ImageUpdateViewModel(Guid.Parse(model.ImageId + ""),
                    eventUpdate.Name, model.Image, CommonConstants.THUMBNAIL);

                    var imageModel = await _imageService.Update(imageUpdateModel);
                    var listImage = await _imageService.GetByKeyIdAndKeyType(Guid.Parse(result.Id + ""), CommonConstants.EVENT_IMAGE);
                    if (listImage == null)
                    {
                        _logger.LogInformation($"{result.Name} has lost image.");
                        throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
                    }
                    var listSourceImage = new List<ImageViewModel>();
                    foreach (var img in listImage)
                    {
                        if (img.Link == null)
                        {
                            _logger.LogInformation($"{result.Name} has lost image.");
                            throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
                        }
                        if (img.Link.Contains(CommonConstants.THUMBNAIL))
                        {
                            result.Thumbnail = img;
                        }
                        else if (img.Link.Contains(CommonConstants.SOURCE_IMAGE))
                        {
                            listSourceImage.Add(img);
                        }
                    }
                    result.ListImage = _mapper.Map<List<EventImageDetailViewModel>>(listSourceImage);
                    return result;
                }
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<ImageViewModel> UpdateImageToEvent(Guid partyId, string roleName, EventUpdateImageViewModel model)
        {
            var img = await _imageService.GetById(model.Id);

            if (!img.KeyType.Equals(CommonConstants.EVENT_IMAGE))
            {
                _logger.LogInformation("You can not update poi image.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "You can not update poi image.");
            }
            var myEvent = await _unitOfWork.EventRepository.Get(e => e.Id.Equals(img.KeyId)).FirstOrDefaultAsync();
           
            if (myEvent == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }

            if (myEvent.Type.Equals(TypeConstants.SERVER_TYPE) && !roleName.Equals(RoleConstants.ADMIN))
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "You can not use this feature.");
            }

            if (myEvent.Type.Equals(TypeConstants.LOCAL_TYPE) && !myEvent.CreatorId.Equals(partyId))
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "You can not use this feature.");
            }

            ImageUpdateViewModel updateModel = 
                new ImageUpdateViewModel(img.Id, myEvent.Name, model.Image, CommonConstants.SOURCE_IMAGE);
           
            var result = await _imageService.Update(updateModel);
            return result;
        }

        public async Task<EventViewModel> ReplaceImage(Guid partyId, string roleName, ImageReplaceViewModel model)
        {
            var checkEvent = await _unitOfWork.EventRepository
                .Get(p => p.Id.Equals(model.Id))
                .ProjectTo<EventViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (checkEvent == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }
            if(model.RemoveFields != null)
            {
                foreach (var imageId in model.RemoveFields)
                {
                    var img = await _imageService.GetById(imageId);
                    if (!img.KeyType.Equals(CommonConstants.EVENT_IMAGE))
                    {
                        _logger.LogInformation("You can not delete other type image.");
                        throw new ErrorResponse((int)HttpStatusCode.BadRequest, "You can not delete other type image.");
                    }
                    var myEvent = await _unitOfWork.EventRepository
                    .Get(p => p.Id.Equals(img.KeyId))
                    .ProjectTo<EventViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                    if (myEvent == null)
                    {
                        _logger.LogInformation("Can not found.");
                        throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
                    }

                    if (myEvent.Type.Equals(TypeConstants.SERVER_TYPE) && !roleName.Equals(RoleConstants.ADMIN))
                    {
                        _logger.LogInformation("You can not use this feature.");
                        throw new ErrorResponse((int)HttpStatusCode.Forbidden, "You can not use this feature.");
                    }

                    if (myEvent.Type.Equals(TypeConstants.LOCAL_TYPE) && !myEvent.CreatorId.Equals(partyId))
                    {
                        _logger.LogInformation("You can not use this feature.");
                        throw new ErrorResponse((int)HttpStatusCode.Forbidden, "You can not use this feature.");
                    }
                    if (img.Link.Contains(CommonConstants.THUMBNAIL))
                    {
                        _logger.LogInformation("You can not delete thumbnail. You can only change thumbnail.");
                        throw new ErrorResponse((int)HttpStatusCode.BadRequest, "You can not delete thumbnail. You can only change thumbnail.");
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
                    ImageCreateViewModel imageModel = new ImageCreateViewModel(checkEvent.Name, image,
                        checkEvent.Id, CommonConstants.EVENT_IMAGE, CommonConstants.SOURCE_IMAGE);
                    var newImage = await _imageService.Create(imageModel);
                }
            }
            var listImage = await _imageService.GetByKeyIdAndKeyType(Guid.Parse(checkEvent.Id + ""), CommonConstants.EVENT_IMAGE);
            if (listImage == null)
            {
                _logger.LogInformation($"{checkEvent.Name} has lost image.");
                throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
            }
            var listSourceImage = new List<ImageViewModel>();
            foreach (var img in listImage)
            {
                if (img.Link == null)
                {
                    _logger.LogInformation($"{checkEvent.Name} has lost image.");
                    throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
                }
                if (img.Link.Contains(CommonConstants.THUMBNAIL))
                {
                    checkEvent.Thumbnail = img;
                }
                else if (img.Link.Contains(CommonConstants.SOURCE_IMAGE))
                {
                    listSourceImage.Add(img);
                }
            }
            checkEvent.ListImage = _mapper.Map<List<EventImageDetailViewModel>>(listSourceImage);
            return checkEvent;
        }
    
    }
}