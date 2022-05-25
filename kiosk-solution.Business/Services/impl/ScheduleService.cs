using System;
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
namespace kiosk_solution.Business.Services.impl
{
    public class ScheduleService : IScheduleService
    {
        private readonly AutoMapper.IConfigurationProvider _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IScheduleService> _logger;

        public ScheduleService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, ILogger<IScheduleService> logger)
        {
            _mapper = mapper.ConfigurationProvider;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<ScheduleViewModel> CreateSchedule(Guid id, CreateScheduleViewModel model)
        {
            var schedule = _mapper.CreateMapper().Map<Schedule>(model);
            schedule.PartyId = id;
            schedule.Status = StatusConstants.OFF;
            schedule.TimeStart=TimeSpan.Parse(model.StringTimeStart);
            schedule.TimeEnd=TimeSpan.Parse(model.StringTimeEnd);
            try
            {
                await _unitOfWork.ScheduleRepository.InsertAsync(schedule);
                await _unitOfWork.SaveAsync();
                var result = _mapper.CreateMapper().Map<ScheduleViewModel>(schedule);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int)HttpStatusCode.UnprocessableEntity, "Invalid Data.");
            }
        }
    }
}