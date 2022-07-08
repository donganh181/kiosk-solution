using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using kiosk_solution.Data.ViewModels;
using AutoMapper;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Repositories;
using kiosk_solution.Data.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace kiosk_solution.Business.Services.impl
{
    public class KioskScheduleTemplateService : IKioskScheduleTemplateService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IKioskScheduleTemplateService> _logger;
        private readonly IScheduleService _scheduleService;
        private readonly ITemplateService _templateService;

        public KioskScheduleTemplateService(IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<IKioskScheduleTemplateService> logger,
            IScheduleService scheduleService, ITemplateService templateService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _scheduleService = scheduleService;
            _templateService = templateService;
        }

        public async Task<KioskScheduleTemplateViewModel> AddTemplateToSchedule(Guid partyId,
            KioskScheduleTemplateCreateViewModel model)
        {
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

            var schedule = await _scheduleService.GetById(model.ScheduleId);
            var conflictData = await _unitOfWork.KioskScheduleTemplateRepository
                .Get(k => k.KioskId.Equals(model.kioskId))
                .Where(a => TimeSpan.Compare((TimeSpan) a.Schedule.TimeStart, (TimeSpan) schedule.TimeStart) == 0
                            && (TimeSpan.Compare((TimeSpan) schedule.TimeStart, (TimeSpan) a.Schedule.TimeStart) == 1 &&
                                TimeSpan.Compare((TimeSpan) schedule.TimeStart, (TimeSpan) a.Schedule.TimeEnd) == -1)
                            && (TimeSpan.Compare((TimeSpan) schedule.TimeStart, (TimeSpan) a.Schedule.TimeStart) ==
                                -1 && TimeSpan.Compare((TimeSpan) schedule.TimeEnd, (TimeSpan) a.Schedule.TimeStart) ==
                                1)).FirstOrDefaultAsync();
            if (conflictData != null)
            {
                _logger.LogInformation("This schedule is conflict with other schedule in this kiosk.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "This schedule is conflict with other schedule in this kiosk.");
            }
            try
            {
                var data = _mapper.Map<KioskScheduleTemplate>(model);
                await _unitOfWork.KioskScheduleTemplateRepository.InsertAsync(data);
                await _unitOfWork.SaveAsync();

                var result = _mapper.Map<KioskScheduleTemplateViewModel>(data);
                return result;
            }
            catch (DbUpdateException)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid data.");
            }
        }
    }
}