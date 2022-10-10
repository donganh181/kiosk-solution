using System;
using System.Collections.Generic;
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
    [Route("api/v{version:apiVersion}/schedules")]
    [ApiController]
    [ApiVersion("1")]
    public class ScheduleController : Controller
    {
        private readonly IScheduleService _scheduleService;
        private readonly ILogger<ScheduleController> _logger;
        private IConfiguration _configuration;
        public ScheduleController(IScheduleService scheduleService, ILogger<ScheduleController> logger, IConfiguration configuration)
        {
            _scheduleService = scheduleService;
            _configuration = configuration;
            _logger = logger;
        }
        [Authorize(Roles = "Location Owner")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CreateSchedule([FromBody] CreateScheduleViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _scheduleService.Create(token.Id, model);
            _logger.LogInformation($"Create schedule {result.Name} by party {token.Mail}.");
            return Ok(new SuccessResponse<ScheduleViewModel>((int) HttpStatusCode.OK, "Create success.", result));
        }
        
        [Authorize(Roles = "Location Owner")]
        [HttpPut]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateSchedule([FromBody] ScheduleUpdateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _scheduleService.Update(token.Id, model);
            _logger.LogInformation($"Update schedule {result.Name} by party {token.Mail}.");
            return Ok(new SuccessResponse<ScheduleViewModel>((int) HttpStatusCode.OK, "Update success.", result));
        }
        
        [Authorize(Roles = "Location Owner")]
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAll(int pageNum, int size)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _scheduleService.GetAllWithPaging(token.Id, size, pageNum);
            _logger.LogInformation($"Get all schedule of party {token.Mail}.");
            return Ok(new SuccessResponse<DynamicModelResponse<ScheduleViewModel>>((int) HttpStatusCode.OK, "Get success.", result));
        }
        
        [Authorize(Roles = "Location Owner")]
        [HttpGet("id")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateSchedule([FromQuery] Guid scheduleId)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _scheduleService.ClientGetById(token.Id, scheduleId);
            _logger.LogInformation($"Get schedule {result.Name} by party {token.Mail}.");
            return Ok(new SuccessResponse<ScheduleViewModel>((int) HttpStatusCode.OK, "Get success.", result));
        }

        [Authorize(Roles = "Location Owner")]
        [HttpPatch("status")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ChangeStatus([FromQuery] Guid scheduleId)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _scheduleService.ChangeStatus(token.Id, scheduleId);
            _logger.LogInformation($"Change status of schedule {result.Name} to {result.Status} by party {token.Mail}.");
            return Ok(new SuccessResponse<ScheduleViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }
    }
}