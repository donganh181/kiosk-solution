using System;
using System.Net;
using System.Threading.Tasks;
using kiosk_solution.Business.Services;
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using kiosk_solution.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/kioskScheduleTemplates")]
    [ApiController]
    [ApiVersion("1")]
    public class KioskScheduleTemplateController : Controller
    {
        private readonly IKioskScheduleTemplateService _kioskScheduleTemplateService;
        private readonly ILogger<KioskScheduleTemplateController> _logger;
        private IConfiguration _configuration;
        public KioskScheduleTemplateController(IKioskScheduleTemplateService kioskScheduleTemplateService, ILogger<KioskScheduleTemplateController> logger, IConfiguration configuration)
        {
            _kioskScheduleTemplateService = kioskScheduleTemplateService;
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
            var result = await _kioskScheduleTemplateService.Create(token.Id, model);
            _logger.LogInformation($"Add template {result.TemplateId} and schedule {model.ScheduleId} to kiosk {model.kioskId} by party {token.Id}.");
            return Ok(new SuccessResponse<KioskScheduleTemplateViewModel>((int) HttpStatusCode.OK,"Add template and schedule to kiosk successful.", result));
        }
        
        [Authorize(Roles = "Location Owner")]
        [HttpPut]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Update([FromBody] KioskScheduleTemplateUpdateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _kioskScheduleTemplateService.Update(token.Id, model);
            _logger.LogInformation($"Update kiosk-schedule-template {model.Id} by party {token.Id}.");
            return Ok(new SuccessResponse<KioskScheduleTemplateViewModel>((int) HttpStatusCode.OK,"Update successful.", result));
        }
        
        [Authorize(Roles = "Location Owner")]
        [HttpDelete]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Delete([FromBody] KioskScheduleTemplateDeleteViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _kioskScheduleTemplateService.Delete(token.Id, model);
            _logger.LogInformation($"Delete kiosk-schedule-template {model.Id} by party {token.Id}.");
            return Ok(new SuccessResponse<KioskScheduleTemplateViewModel>((int) HttpStatusCode.OK,"Delete successful.", result));
        }
        
        [Authorize(Roles = "Location Owner")]
        [HttpGet("kioskId")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetByKioskId([FromQuery] Guid kioskId, int size, int page = CommonConstants.DefaultPage)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _kioskScheduleTemplateService.GetByKioskId(kioskId, token.Id, size, page);
            _logger.LogInformation($"Get template and schedule of kiosk {kioskId} by party {token.Id}.");
            return Ok(new SuccessResponse<DynamicModelResponse<KioskScheduleTemplateViewModel>>((int) HttpStatusCode.OK,"Get success.", result));
        }

        [Authorize(Roles = "Location Owner")]
        [HttpPatch("status")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ChangeStatus([FromQuery] Guid kioskScheduleTemplateId)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _kioskScheduleTemplateService.ChangeStatus(token.Id, kioskScheduleTemplateId);
            _logger.LogInformation($"Change status of kiosk schedule template id {result.Id} to Status {result.Status} by party {token.Id}.");
            return Ok(new SuccessResponse<KioskScheduleTemplateViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        [Authorize(Roles = "Location Owner")]
        [HttpPatch("status-by-templateId")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ChangeStatusByTemplateId([FromQuery] Guid templateId)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _kioskScheduleTemplateService.ChangeStatusByTemplateId(token.Id, templateId);
            foreach(var target in result)
            {
                _logger.LogInformation($"Change status of kiosk schedule template id {target.Id} to Status {target.Status} by party {token.Id}.");
            }
            return Ok(new SuccessResponse<List<KioskScheduleTemplateViewModel>>((int)HttpStatusCode.OK, "Update success.", result));
        }

        [Authorize(Roles = "Location Owner")]
        [HttpPatch("status-by-scheduleId")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ChangeStatusByScheduleId([FromQuery] Guid schedule)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _kioskScheduleTemplateService.ChangeStatusByScheduleId(token.Id, schedule);
            foreach (var target in result)
            {
                _logger.LogInformation($"Change status of kiosk schedule template id {target.Id} to Status {target.Status} by party {token.Id}.");
            }
            return Ok(new SuccessResponse<List<KioskScheduleTemplateViewModel>>((int)HttpStatusCode.OK, "Update success.", result));
        }
    }
}