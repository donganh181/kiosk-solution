using kiosk_solution.Business.Services;
using kiosk_solution.Data.Responses;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Business.SystemSchedule.Jobs
{
    public class CheckEventJob : IJob
    {
        private readonly ILogger<CheckEventJob> _logger;
        private readonly IEventService _eventService;

        public CheckEventJob(ILogger<CheckEventJob> logger, IEventService eventService)
        {
            _logger = logger;
            _eventService = eventService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var check = await _eventService.ValidateStatusOfEventByDay();
            if (!check)
            {
                _logger.LogInformation("Server Error.");
            }
            _logger.LogInformation("check template job running...");
        }
    }
}
