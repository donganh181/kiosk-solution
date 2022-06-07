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

        public EventService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<IEventService> logger, IImageService imageService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _imageService = imageService;
        }

        public async Task<EventViewModel> Create(Guid creatorId, string role, EventCreateViewModel model)
        {
            
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
                newEvent.Type = CommonConstants.SERVER_TYPE;
            }
            else if (role.Equals(RoleConstants.LOCATION_OWNER))
            {
                newEvent.Type = CommonConstants.LOCAL_TYPE;
            }
            else
            {
                _logger.LogInformation("Server Error.");
                throw new ErrorResponse((int) HttpStatusCode.InternalServerError, "Server Error.");
            }

            try
            {                
                await _unitOfWork.EventRepository.InsertAsync(newEvent);
                await _unitOfWork.SaveAsync();

                ImageCreateViewModel imageModel = new ImageCreateViewModel(newEvent.Name, model.Image, newEvent.Id, CommonConstants.EVENT_IMAGE, CommonConstants.THUMBNAIL);

                var img = await _imageService.Create(imageModel);

                var result = await _unitOfWork.EventRepository
                    .Get(e => e.Id.Equals(newEvent.Id))
                    .Include(e => e.Creator)
                    .ProjectTo<EventViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                result.Link = img.Link;
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int) HttpStatusCode.UnprocessableEntity, "Invalid Data.");
            }
        }

        public async Task<DynamicModelResponse<EventSearchViewModel>> GetAllWithPaging(Guid partyId, string roleName,
            EventSearchViewModel model, int size, int pageNum)
        {
            IOrderedQueryable<EventSearchViewModel> events = null;
            if (roleName.Equals(RoleConstants.LOCATION_OWNER))
            {
                events = _unitOfWork.EventRepository
                    .Get(e => (e.CreatorId.Equals(partyId) &&
                               e.Type.Equals(CommonConstants.LOCAL_TYPE)) ||
                              e.Type.Equals(CommonConstants.SERVER_TYPE))
                    .Include(e => e.Creator)
                    .ProjectTo<EventSearchViewModel>(_mapper.ConfigurationProvider)
                    .DynamicFilter(model)
                    .AsQueryable().OrderByDescending(t => t.Name);
            }
            else if(roleName.Equals(RoleConstants.ADMIN))
            {
                events = _unitOfWork.EventRepository
                    .Get()
                    .Include(e => e.Creator)
                    .ProjectTo<EventSearchViewModel>(_mapper.ConfigurationProvider)
                    .DynamicFilter(model)
                    .AsQueryable().OrderByDescending(t => t.Name);
            }

            var listPaging = events.PagingIQueryable(pageNum, size, CommonConstants.LimitPaging,
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

            if (eventUpdate.Type.Equals(CommonConstants.SERVER_TYPE) && !roleName.Equals(RoleConstants.ADMIN))
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "You can not use this feature.");
            }
            if (eventUpdate.Type.Equals(CommonConstants.LOCAL_TYPE) && !eventUpdate.CreatorId.Equals(partyId))
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "You can not use this feature.");
            }
            eventUpdate.Name = model.Name;
            eventUpdate.Description = model.Description;
            eventUpdate.Address = model.Address;
            eventUpdate.TimeStart = model.TimeStart;
            eventUpdate.TimeEnd = model.TimeEnd;
            eventUpdate.City = model.City;
            eventUpdate.District = model.District;
            eventUpdate.Latitude = model.Latitude;
            eventUpdate.Longtitude = model.Longtitude;
            eventUpdate.Ward = model.Ward;
            eventUpdate.Street = model.Street;

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
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int) HttpStatusCode.UnprocessableEntity, "Invalid data.");
            }
        }
    }
}