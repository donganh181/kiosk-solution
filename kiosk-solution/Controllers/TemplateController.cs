using kiosk_solution.Business.Services;
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using kiosk_solution.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/templates")]
    [ApiController]
    [ApiVersion("1")]
    public class TemplateController : Controller
    {
        private readonly ITemplateService _templateService;
        private readonly ILogger<TemplateController> _logger;
        private IConfiguration _configuration;
        public TemplateController(ITemplateService templateService, ILogger<TemplateController> logger, IConfiguration configuration)
        {
            _templateService = templateService;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Create template by location owner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Location Owner")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create([FromBody] TemplateCreateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _templateService.Create(token.Id, model);
            _logger.LogInformation($"Create template {result.Name} by party {token.Mail}");
            return Ok(new SuccessResponse<TemplateViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }

        /// <summary>
        /// Get all of its template by location owner
        /// </summary>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Authorize(Roles = "Location Owner")]
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Get([FromQuery] TemplateSearchViewModel model, int size, int page = CommonConstants.DefaultPage)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            Guid id = token.Id;
            string role = token.Role;
            var result = await _templateService.GetAllWithPaging(id, model, size, page);
            _logger.LogInformation($"Get all templates by party {token.Mail}");
            return Ok(new SuccessResponse<DynamicModelResponse<TemplateSearchViewModel>>((int)HttpStatusCode.OK, "Search success.", result));
        }

        /// <summary>
        /// Update template information by its location owner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Location Owner")]
        [HttpPut]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateInformation([FromBody] TemplateUpdateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            Guid updaterId = token.Id;
            var result = await _templateService.UpdateInformation(updaterId, model);
            _logger.LogInformation($"Updated template {result.Name} by party {token.Mail}");
            return Ok(new SuccessResponse<TemplateViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        [Authorize(Roles = "Location Owner")]
        [HttpDelete]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateStatus(Guid templateId)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _templateService.UpdateStatus(token.Id, templateId);
            _logger.LogInformation($"Update status of template [{result.Id}] by party {token.Mail}");
            return Ok(new SuccessResponse<TemplateViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }
        
        [HttpGet("id")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetById(Guid templateId)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _templateService.GetById(templateId);
            _logger.LogInformation($"Get template by id success");
            return Ok(new SuccessResponse<TemplateViewModel>((int)HttpStatusCode.OK, "Get success.", result));
        }
    }
}
