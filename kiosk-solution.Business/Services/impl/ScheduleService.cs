using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using kiosk_solution.Data.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using kiosk_solution.Business.Utilities;
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Repositories;
using kiosk_solution.Data.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace kiosk_solution.Business.Services.impl
{
    public class ScheduleService : IScheduleService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IScheduleService> _logger;

        public ScheduleService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<IScheduleService> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ScheduleViewModel> Create(Guid partyId, CreateScheduleViewModel model)
        {
            var schedule = _mapper.Map<Schedule>(model);
            schedule.PartyId = partyId;
            schedule.Status = StatusConstants.OFF;
            schedule.TimeStart = TimeSpan.Parse(model.StringTimeStart);
            schedule.TimeEnd = TimeSpan.Parse(model.StringTimeEnd);
            try
            {
                await _unitOfWork.ScheduleRepository.InsertAsync(schedule);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<ScheduleViewModel>(schedule);
                result.TimeStart = schedule.TimeStart.ToString();
                result.TimeEnd = schedule.TimeEnd.ToString();
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid Data.");
            }
        }

        public async Task<ScheduleViewModel> Update(Guid partyId, ScheduleUpdateViewModel model)
        {
            var schedule = await _unitOfWork.ScheduleRepository.Get(s => s.Id.Equals(model.Id)).FirstOrDefaultAsync();
            if (schedule == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Can not found.");
            }

            if (!schedule.PartyId.Equals(partyId))
            {
                _logger.LogInformation("You cannot update schedule of other user.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "You cannot update schedule of other user.");
            }
            schedule.Name = model.Name;
            schedule.TimeStart = TimeSpan.Parse(model.StringTimeStart);
            schedule.TimeEnd = TimeSpan.Parse(model.StringTimeEnd);
            schedule.DayOfWeek = model.DayOfWeek;
            try
            {
                _unitOfWork.ScheduleRepository.Update(schedule);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<ScheduleViewModel>(schedule);
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<DynamicModelResponse<ScheduleViewModel>> GetAllWithPaging(Guid id, int size, int pageNum)
        {
            var list = _unitOfWork.ScheduleRepository.Get(s => s.PartyId.Equals(id))
                .ProjectTo<ScheduleViewModel>(_mapper.ConfigurationProvider).OrderByDescending(x => x.Name);
            var listPaging =
                list.PagingIQueryable(pageNum, size, CommonConstants.LimitPaging, CommonConstants.DefaultPaging);
            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Cannot found");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "You don't have any schedule.");
            }

            var result = new DynamicModelResponse<ScheduleViewModel>
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

        public async Task<bool> IsOwner(Guid partyId, Guid scheduleId)
        {
            var schedule = await _unitOfWork.ScheduleRepository.Get(s => s.Id.Equals(scheduleId)).FirstOrDefaultAsync();
            if (schedule == null)
            {
                _logger.LogInformation($"Schedule {scheduleId} is not exist.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Schedule is not exist.");
            }

            bool result = schedule.PartyId.Equals(partyId);
            return result;
        }

        public async Task<Schedule> GetById(Guid scheduleId)
        {
            var sch = await _unitOfWork.ScheduleRepository.Get(s => s.Id.Equals(scheduleId)).FirstOrDefaultAsync();
            return sch;
        }

        public async Task<ScheduleViewModel> ClientGetById(Guid partyId, Guid scheduleId)
        {
            var schedule = await _unitOfWork.ScheduleRepository
                .Get(s => s.Id.Equals(scheduleId) && s.PartyId.Equals(partyId)).FirstOrDefaultAsync();
            if (schedule == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Can not found.");
            }
            var result = _mapper.Map<ScheduleViewModel>(schedule);
            return result;
        }

        public async Task<ScheduleViewModel> ChangeStatus(Guid partyId, Guid scheduleId)
        {
            var schedule = await _unitOfWork.ScheduleRepository
                .Get(s => s.Id.Equals(scheduleId))
                .FirstOrDefaultAsync();

            if (!schedule.PartyId.Equals(partyId))
            {
                _logger.LogInformation("You cannot update schedule of other user.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "You cannot update schedule of other user.");
            }

            if (schedule.Status.Equals(StatusConstants.ON))
            {
                schedule.Status = StatusConstants.OFF;
            }
            else
            {
                schedule.Status = StatusConstants.ON;
            }
            try
            {
                _unitOfWork.ScheduleRepository.Update(schedule);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.ScheduleRepository
                .Get(s => s.Id.Equals(scheduleId))
                .ProjectTo<ScheduleViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }
    }
}