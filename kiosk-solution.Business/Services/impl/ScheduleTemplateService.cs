using System;
using System.Collections.Generic;
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace kiosk_solution.Business.Services.impl
{
    public class ScheduleTemplateService
    {
        private readonly AutoMapper.IConfigurationProvider _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IScheduleTemplateService> _logger;

        public ScheduleTemplateService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration,
            ILogger<IScheduleTemplateService> logger)
        {
            _mapper = mapper.ConfigurationProvider;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        
        public async Task AddTemplateToSchedule(Guid partyId, AddTemplateViewModel model)
        {
            var user = await _unitOfWork.PartyRepository.Get(u => u.Id.Equals(partyId)).FirstOrDefaultAsync();
            var schedule = await _unitOfWork.ScheduleRepository.Get(s => s.Id.Equals(model.ScheduleId)).FirstOrDefaultAsync();
            var template = await _unitOfWork.TemplateRepository.Get(t => t.Id.Equals(model.TemplateId))
                .FirstOrDefaultAsync();
            if (!schedule.PartyId.Equals(partyId))
            {
                _logger.LogInformation($"{user.Email} account cannot use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "Your account cannot use this feature.");
            }
            if (!template.PartyId.Equals(partyId))
            {
                _logger.LogInformation($"{user.Email} account cannot use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "Your account cannot use this feature.");
            }

            try
            {
                var data = _mapper.CreateMapper().Map<ScheduleTemplate>(model);
                await _unitOfWork.ScheduleTemplateRepository.InsertAsync(data);
                await _unitOfWork.SaveAsync();
            }
            catch (DbUpdateException)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int) HttpStatusCode.UnprocessableEntity, "Invalid data.");
            }
        }
    }
    
}