﻿using System;
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
    public class ScheduleTemplateService : IScheduleTemplateService
    {
        private readonly AutoMapper.IConfigurationProvider _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IScheduleTemplateService> _logger;
        private readonly IScheduleService _scheduleService;
        private readonly ITemplateService _templateService;

        public ScheduleTemplateService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<IScheduleTemplateService> logger,
            IScheduleService scheduleService, ITemplateService templateService)
        {
            _mapper = mapper.ConfigurationProvider;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _scheduleService = scheduleService;
            _templateService = templateService;
        }

        public async Task<AddTemplateViewModel> AddTemplateToSchedule(Guid partyId, AddTemplateViewModel model)
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

            try
            {
                var data = _mapper.CreateMapper().Map<ScheduleTemplate>(model);
                await _unitOfWork.ScheduleTemplateRepository.InsertAsync(data);
                await _unitOfWork.SaveAsync();
                return model;
            }
            catch (DbUpdateException)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int) HttpStatusCode.UnprocessableEntity, "Invalid data.");
            }
        }
    }
}