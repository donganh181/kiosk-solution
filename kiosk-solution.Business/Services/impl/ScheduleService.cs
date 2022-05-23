using System;
using System.Threading.Tasks;
using kiosk_solution.Data.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using kiosk_solution.Business.Utilities;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
namespace kiosk_solution.Business.Services.impl
{
    public class ScheduleService : IScheduleService
    {
        private readonly AutoMapper.IConfigurationProvider _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IPartyService> _logger;

        public ScheduleService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, ILogger<IPartyService> logger)
        {
            _mapper = mapper.ConfigurationProvider;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<ScheduleViewModel> CreateSchedule(Guid id, CreateScheduleViewModel model)
        {
            var schedule = _mapper.CreateMapper().Map<Schedule>(model);

            ScheduleViewModel x = new ScheduleViewModel();
            return x;
        }
    }
}