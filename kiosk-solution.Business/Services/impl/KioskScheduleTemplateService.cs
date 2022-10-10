using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using kiosk_solution.Data.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using kiosk_solution.Business.Hubs;
using kiosk_solution.Business.Utilities;
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Repositories;
using kiosk_solution.Data.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace kiosk_solution.Business.Services.impl
{
    public class KioskScheduleTemplateService : IKioskScheduleTemplateService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IKioskScheduleTemplateService> _logger;
        private readonly IScheduleService _scheduleService;
        private readonly ITemplateService _templateService;
        private readonly IKioskService _kioskService;
        private readonly INotiService _fcmService;
        private readonly IEventService _eventService;
        private readonly IHubContext<SystemEventHub> _eventHub;

        public KioskScheduleTemplateService(IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<IKioskScheduleTemplateService> logger,
            IScheduleService scheduleService, ITemplateService templateService, IKioskService kioskService,
            INotiService fcmService, IEventService eventService, IHubContext<SystemEventHub> eventHub)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _scheduleService = scheduleService;
            _templateService = templateService;
            _kioskService = kioskService;
            _fcmService = fcmService;
            _eventService = eventService;
            _eventHub = eventHub;
        }

        public async Task<KioskScheduleTemplateViewModel> Create(Guid partyId,
            KioskScheduleTemplateCreateViewModel model)
        {
            var now = DateTime.Now;
            var timeNow = now.TimeOfDay;
            var thisDay = now.ToString("dddd");

            bool isScheduleOwner = await _scheduleService.IsOwner(partyId, (Guid) model.ScheduleId);
            bool isTemplateOwner = await _templateService.IsOwner(partyId, (Guid) model.TemplateId);
            if (!isScheduleOwner)
            {
                _logger.LogInformation($"{partyId} account cannot use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "Your account cannot use this feature.");
            }

            if (!isTemplateOwner)
            {
                _logger.LogInformation($"{partyId} account cannot use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "Your account cannot use this feature.");
            }

            var kioskScheduleTemplate = await _unitOfWork.KioskScheduleTemplateRepository.Get(x =>
                x.ScheduleId.Equals(model.ScheduleId) && x.TemplateId.Equals(model.TemplateId) &&
                x.KioskId.Equals(model.kioskId)).FirstOrDefaultAsync();
            if (kioskScheduleTemplate != null)
            {
                _logger.LogInformation("This template and schedule are already set for kiosk.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest,
                    "This template and schedule are already set for kiosk.");
            }

            var template = await _templateService.GetById(model.TemplateId);
            if (template.Status.Equals(StatusConstants.INCOMPLETE))
            {
                _logger.LogInformation("This template is incomplete.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest,
                    "This template is incomplete.");
            }

            var schedule = await _scheduleService.GetById(model.ScheduleId);
            var listDay = schedule.DayOfWeek.Split("-").ToList();
            var conflictData = await _unitOfWork.KioskScheduleTemplateRepository
                .Get(k => k.KioskId.Equals(model.kioskId))
                .Where(a => TimeSpan.Compare((TimeSpan) a.Schedule.TimeStart, (TimeSpan) schedule.TimeStart) == 0
                            || (TimeSpan.Compare((TimeSpan) schedule.TimeStart, (TimeSpan) a.Schedule.TimeStart) == 1 &&
                                TimeSpan.Compare((TimeSpan) schedule.TimeStart, (TimeSpan) a.Schedule.TimeEnd) == -1)
                            || (TimeSpan.Compare((TimeSpan) schedule.TimeStart, (TimeSpan) a.Schedule.TimeStart) ==
                                -1 && TimeSpan.Compare((TimeSpan) schedule.TimeEnd, (TimeSpan) a.Schedule.TimeStart) ==
                                1)).Include(x => x.Template).Include(x => x.Schedule)
                .ProjectTo<KioskScheduleTemplateViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            if (conflictData.Count != 0)
            {
                foreach (var x in conflictData)
                {
                    if (listDay.Any(s => x.Schedule.DayOfWeek.Contains(s)))
                    {
                        _logger.LogInformation(
                            "This schedule is conflict with other schedule in this kiosk.");
                        throw new ErrorResponse((int) HttpStatusCode.BadRequest,
                            "This schedule is conflict with other schedule in this kiosk.");
                    }
                }
            }
            
            try
            {
                var data = _mapper.Map<KioskScheduleTemplate>(model);
                data.Status = StatusConstants.ACTIVATE;
                await _unitOfWork.KioskScheduleTemplateRepository.InsertAsync(data);
                await _unitOfWork.SaveAsync();

                if(schedule.DayOfWeek.Contains(thisDay) 
                    && TimeSpan.Compare(timeNow, (TimeSpan)schedule.TimeStart) >= 0
                    && TimeSpan.Compare(timeNow, (TimeSpan)schedule.TimeEnd) < 0)
                {
                    var tmp = await _unitOfWork.KioskScheduleTemplateRepository
                    .Get(x => x.Id.Equals(data.Id))
                    .Include(a => a.Kiosk)
                    .Include(a => a.Schedule)
                    .Include(a => a.Template)
                    .ThenInclude(b => b.AppCategoryPositions)
                    .ThenInclude(c => c.AppCategory)
                    .Include(a => a.Template)
                    .ThenInclude(b => b.EventPositions)
                    .ThenInclude(c => c.Event)
                    .ProjectTo<KioskScheduleTemplateChangeViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                    foreach(var eventPos in tmp.Template.ListEventPosition)
                    {
                        var myEvent = await _eventService.GetById(eventPos.EventId);
                        eventPos.EventThumbnail = myEvent.Thumbnail;
                    }

                    await _eventHub.Clients.Group(tmp.KioskId.ToString())
                        .SendAsync(SystemEventHub.KIOSK_CONNECTION_CHANNEL,
                        SystemEventHub.SYSTEM_BOT, tmp.Template);
                }
                var result = await _unitOfWork.KioskScheduleTemplateRepository.Get(x => x.Id.Equals(data.Id))
                    .Include(x => x.Template).Include(x => x.Schedule)
                    .ProjectTo<KioskScheduleTemplateViewModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<KioskScheduleTemplateViewModel> Delete(Guid partyId, KioskScheduleTemplateDeleteViewModel model)
        {
            var target = await _unitOfWork.KioskScheduleTemplateRepository.Get(k => k.Id.Equals(model.Id))
                .FirstOrDefaultAsync();
            if (target == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Can not found.");
            }

            try
            {
                _unitOfWork.KioskScheduleTemplateRepository.Delete(target);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<KioskScheduleTemplateViewModel>(target);
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<KioskScheduleTemplateViewModel> Update(Guid partyId, KioskScheduleTemplateUpdateViewModel model)
        {
            var target = await _unitOfWork.KioskScheduleTemplateRepository.Get(k => k.Id.Equals(model.Id))
                .FirstOrDefaultAsync();
            if (target == null)
            {
                _logger.LogInformation($"Can not found.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Can not found.");
            }
            bool isScheduleOwner = await _scheduleService.IsOwner(partyId, (Guid) model.ScheduleId);
            bool isTemplateOwner = await _templateService.IsOwner(partyId, (Guid) model.TemplateId);
            if (!isScheduleOwner)
            {
                _logger.LogInformation($"{partyId} account cannot use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "Your account cannot use this feature.");
            }

            if (!isTemplateOwner)
            {
                _logger.LogInformation($"{partyId} account cannot use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "Your account cannot use this feature.");
            }
            var schedule = await _scheduleService.GetById(model.ScheduleId);
            var listDay = schedule.DayOfWeek.Split("-").ToList();
            var conflictData = await _unitOfWork.KioskScheduleTemplateRepository
                .Get(k => k.KioskId.Equals(target.KioskId) && !k.Id.Equals(model.Id))
                .Where(a => TimeSpan.Compare((TimeSpan) a.Schedule.TimeStart, (TimeSpan) schedule.TimeStart) == 0
                            || (TimeSpan.Compare((TimeSpan) schedule.TimeStart, (TimeSpan) a.Schedule.TimeStart) == 1 &&
                                TimeSpan.Compare((TimeSpan) schedule.TimeStart, (TimeSpan) a.Schedule.TimeEnd) == -1)
                            || (TimeSpan.Compare((TimeSpan) schedule.TimeStart, (TimeSpan) a.Schedule.TimeStart) ==
                                -1 && TimeSpan.Compare((TimeSpan) schedule.TimeEnd, (TimeSpan) a.Schedule.TimeStart) ==
                                1)).Include(x => x.Template).Include(x => x.Schedule)
                .ProjectTo<KioskScheduleTemplateViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            if (conflictData.Count != 0)
            {
                foreach (var x in conflictData)
                {
                    if (listDay.Any(s => x.Schedule.DayOfWeek.Contains(s)))
                    {
                        _logger.LogInformation(
                            "This schedule is conflict with other schedule in this kiosk.");
                        throw new ErrorResponse((int) HttpStatusCode.BadRequest,
                            "This schedule is conflict with other schedule in this kiosk.");
                    }
                }
            }
            
            try
            {
                target.ScheduleId = model.ScheduleId;
                target.TemplateId = model.TemplateId;
                _unitOfWork.KioskScheduleTemplateRepository.Update(target);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.KioskScheduleTemplateRepository.Get(x => x.Id.Equals(target.Id))
                    .Include(x => x.Template).Include(x => x.Schedule)
                    .ProjectTo<KioskScheduleTemplateViewModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
                return result;
            }
            catch (DbUpdateException)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<DynamicModelResponse<KioskScheduleTemplateViewModel>> GetByKioskId(Guid kioskId, Guid partyId,
            int size,
            int pageNum)
        {
            var kiosk = await _kioskService.GetById(kioskId);
            if (kiosk == null)
            {
                _logger.LogInformation("Can not found kiosk.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Can not found kiosk.");
            }

            if (!partyId.Equals(kiosk.PartyId))
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "You can not use this feature.");
            }

            var listObj = _unitOfWork.KioskScheduleTemplateRepository.Get(x => x.KioskId.Equals(kioskId))
                .Include(x => x.Template).Include(x => x.Schedule)
                .ProjectTo<KioskScheduleTemplateViewModel>(_mapper.ConfigurationProvider).PagingIQueryable(pageNum,
                    size, CommonConstants.LimitPaging, CommonConstants.DefaultPaging);
            if (listObj.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not found");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not found.");
            }

            var result = new DynamicModelResponse<KioskScheduleTemplateViewModel>
            {
                Metadata = new PagingMetaData
                {
                    Page = pageNum,
                    Size = size,
                    Total = listObj.Total
                },
                Data = listObj.Data.ToList()
            };
            return result;
        }

        public async Task<KioskScheduleTemplateViewModel> ChangeStatus(Guid partyId, Guid kioskScheduleTemplateId)
        {
            var target = await _unitOfWork.KioskScheduleTemplateRepository
                .Get(a => a.Id.Equals(kioskScheduleTemplateId))
                .Include(x => x.Kiosk)
                .FirstOrDefaultAsync();

            if(target == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }

            if (!target.Kiosk.PartyId.Equals(partyId))
            {
                _logger.LogInformation("You cannot update kiosk schedule template of other user.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "You cannot update kiosk schedule template of other user.");
            }
            if (target.Status.Equals(StatusConstants.ACTIVATE))
            {
                target.Status = StatusConstants.DEACTIVATE;
            }
            else
            {
                target.Status = StatusConstants.ACTIVATE;
            }
            try
            {
                _unitOfWork.KioskScheduleTemplateRepository.Update(target);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.KioskScheduleTemplateRepository
                .Get(a => a.Id.Equals(kioskScheduleTemplateId))
                .Include(x => x.Kiosk)
                .ProjectTo<KioskScheduleTemplateViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<List<KioskScheduleTemplateViewModel>> ChangeStatusByTemplateId(Guid partyId, Guid templateId)
        {
            var listTarget = await _unitOfWork.KioskScheduleTemplateRepository
                .Get(t => t.TemplateId.Equals(templateId))
                .Include(a => a.Kiosk)
                .Include(a => a.Template)
                .ToListAsync();

            if (listTarget.Count < 1)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }
            if (!listTarget.First().Template.Status.Equals(StatusConstants.DELETED))
            {
                _logger.LogInformation("You cannot change status of this target (this template is not deleted).");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "You cannot change status of this target (this template is not deleted).");
            }

            try
            {
                foreach (var target in listTarget)
                {
                    if (!target.Kiosk.PartyId.Equals(partyId))
                    {
                        _logger.LogInformation("You cannot update kiosk schedule template of other user.");
                        throw new ErrorResponse((int)HttpStatusCode.Forbidden, "You cannot update kiosk schedule template of other user.");
                    }

                    target.Status = StatusConstants.DEACTIVATE;
                    _unitOfWork.KioskScheduleTemplateRepository.Update(target);
                }
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.KioskScheduleTemplateRepository
                .Get(a => a.TemplateId.Equals(templateId))
                .Include(x => x.Kiosk)
                .ProjectTo<KioskScheduleTemplateViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<List<KioskScheduleTemplateViewModel>> ChangeStatusByScheduleId(Guid partyId, Guid scheduleId)
        {
            var listTarget = await _unitOfWork.KioskScheduleTemplateRepository
                .Get(t => t.ScheduleId.Equals(scheduleId))
                .Include(a => a.Kiosk)
                .Include(a => a.Schedule)
                .ToListAsync();

            if (listTarget.Count < 1)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }
            if (!listTarget.First().Schedule.Status.Equals(StatusConstants.OFF))
            {
                _logger.LogInformation("You cannot change status of this target (schedule status is ON).");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "You cannot change status of this target (schedule status is ON).");
            }
            try
            {
                foreach (var target in listTarget)
                {
                    if (!target.Kiosk.PartyId.Equals(partyId))
                    {
                        _logger.LogInformation("You cannot update kiosk schedule template of other user.");
                        throw new ErrorResponse((int)HttpStatusCode.Forbidden, "You cannot update kiosk schedule template of other user.");
                    }

                    target.Status = StatusConstants.DEACTIVATE;
                    _unitOfWork.KioskScheduleTemplateRepository.Update(target);
                }
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.KioskScheduleTemplateRepository
                .Get(a => a.ScheduleId.Equals(scheduleId))
                .Include(x => x.Kiosk)
                .ProjectTo<KioskScheduleTemplateViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

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