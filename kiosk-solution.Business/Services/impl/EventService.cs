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

namespace kiosk_solution.Business.Services.impl
{
    public class EventService : IEventService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IEventService> _logger;

        public EventService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<IEventService> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<EventViewModel> Create(Guid creatorId, string role, EventCreateViewModel model)
        {
            var newEvent = _mapper.Map<Event>(model);

            newEvent.CreatorId = creatorId;

            var TimeStart = DateTime.Parse(newEvent.TimeStart + "");
            var TimeEnd = DateTime.Parse(newEvent.TimeEnd + "");

            //case time start = time end or time start > time end
            if (DateTime.Compare(TimeStart,TimeEnd) == 0 || DateTime.Compare(TimeStart, TimeEnd) > 0)
            {
                _logger.LogInformation("Time start cannot happen at the same time or later than time end.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Time start cannot happen at the same time or later than time end.");
            }
            ///case now > time end
            if (DateTime.Compare(DateTime.Now, TimeEnd) > 0 )
            {
                _logger.LogInformation("This event is end.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This event is end.");
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
                throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Server Error.");
            }
            try
            {
                await _unitOfWork.EventRepository.InsertAsync(newEvent);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<EventViewModel>(newEvent);
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int)HttpStatusCode.UnprocessableEntity, "Invalid Data.");
            }


        }
    }
}
