using System.Net;
using System.Threading.Tasks;
using kiosk_solution.Business.Services;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using kiosk_solution.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/kioskScheduleTemplates")]
    [ApiController]
    [ApiVersion("1")]
    public class KioskScheduleTemplateController : Controller
    {
        private readonly IKioskScheduleTemplateService _scheduleTemplateService;
        private readonly ILogger<KioskScheduleTemplateController> _logger;
        private IConfiguration _configuration;
        public KioskScheduleTemplateController(IKioskScheduleTemplateService scheduleTemplateService, ILogger<KioskScheduleTemplateController> logger, IConfiguration configuration)
        {
            _scheduleTemplateService = scheduleTemplateService;
            _configuration = configuration;
            _logger = logger;
        }

        [Authorize(Roles = "Location Owner")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> AddTemplateScheduleToKiosk([FromBody] KioskScheduleTemplateCreateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _scheduleTemplateService.AddTemplateToSchedule(token.Id, model);
            _logger.LogInformation($"Add template {result.TemplateId} and schedule {model.ScheduleId} to kiosk {model.kioskId} by party {token.Id}.");
            return Ok(new SuccessResponse<KioskScheduleTemplateViewModel>((int) HttpStatusCode.OK,"Add template and schedule to kiosk successful.", result));
        }
    }
}