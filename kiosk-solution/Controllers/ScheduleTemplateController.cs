using kiosk_solution.Business.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace kiosk_solution.Controllers
{
    public class ScheduleTemplateController : Controller
    {
        private readonly IScheduleTemplateService _scheduleTemplateService;
        private readonly ILogger<ScheduleTemplateController> _logger;
        private IConfiguration _configuration;
        public ScheduleTemplateController(IScheduleTemplateService scheduleTemplateService, ILogger<ScheduleTemplateController> logger, IConfiguration configuration)
        {
            _scheduleTemplateService = scheduleTemplateService;
            _configuration = configuration;
            _logger = logger;
        }
    }
}