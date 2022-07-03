using System;
using System.Net;
using System.Threading.Tasks;
using kiosk_solution.Business.Services;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using kiosk_solution.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/eventPositions")]
    [ApiController]
    [ApiVersion("1")]
    public class EventPositionController : Controller
    {
        private readonly IEventPositionService _eventPositionService;
        private readonly ILogger _logger;
        private IConfiguration _configuration;
        public EventPositionController(IEventPositionService eventPositionService, ILogger<EventPositionController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _eventPositionService = eventPositionService;
            _configuration = configuration;
        }
        
        [Authorize(Roles = "Location Owner")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create([FromBody] EventPositionCreateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _eventPositionService.Create(token.Id ,model);
            _logger.LogInformation($"Create successfuly to template {result.TemplateName} by party {token.Mail}");
            return Ok(new SuccessResponse<EventPositionViewModel>((int)HttpStatusCode.OK, "Add to template success.", result));
        }

        [Authorize(Roles = "Location Owner")]
        [HttpPut]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdatePosition([FromBody] EventPositionUpdateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _eventPositionService.Update(token.Id, model);
            _logger.LogInformation($"Update successfuly to template {result.TemplateName} by party {token.Mail}");
            return Ok(new SuccessResponse<EventPositionViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }
        
        [Authorize(Roles = "Location Owner")]
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetPosition([FromQuery] Guid templateId)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _eventPositionService.GetById(token.Id, templateId);
            _logger.LogInformation($"Get event position {result.TemplateName} by party {token.Mail} successful");
            return Ok(new SuccessResponse<EventPositionGetViewModel>((int)HttpStatusCode.OK, "Get success.", result));
        }
    }
}