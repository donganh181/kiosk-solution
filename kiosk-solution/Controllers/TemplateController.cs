using kiosk_solution.Business.Services;
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
    }
}
