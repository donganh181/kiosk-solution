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
    [Route("api/v{version:apiVersion}/feedbacks")]
    [ApiController]
    [ApiVersion("1")]
    public class ServiceApplicationFeedBackController : Controller
    {
        private readonly IServiceApplicationFeedBackService _serviceApplicationFeedBackService;
        private readonly ILogger<ServiceApplicationFeedBackController> _logger;
        private IConfiguration _configuration;

        public ServiceApplicationFeedBackController(IServiceApplicationFeedBackService serviceApplicationFeedBackService, ILogger<ServiceApplicationFeedBackController> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _serviceApplicationFeedBackService = serviceApplicationFeedBackService;
            _configuration = configuration;
        }

        /// <summary>
        /// Create feedback by location owner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Location Owner")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create([FromBody] ServiceApplicationFeedBackCreateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _serviceApplicationFeedBackService.Create(token.Id, model);
            _logger.LogInformation($"Create feedback success to app {result.ServiceApplicationName} by party {token.Mail}");
            return Ok(new SuccessResponse<ServiceApplicationFeedBackViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }

        /// <summary>
        /// Update feedback by its location owner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Location Owner")]
        [HttpPut]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Update([FromBody] ServiceApplicationFeedBackUpdateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _serviceApplicationFeedBackService.Update(token.Id, model);
            _logger.LogInformation($"Updated feedback id {result.Id} to app {result.ServiceApplicationName} by party {token.Mail}");
            return Ok(new SuccessResponse<ServiceApplicationFeedBackViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// Get list feedback by applicationId
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="size"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Location Owner, Service Provider")]
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetListFeedbackByAppId([FromQuery] Guid appId, int size, int page = CommonConstants.DefaultPage)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _serviceApplicationFeedBackService.GetListFeedbackByAppIdWithPaging(appId, size, page);
            _logger.LogInformation($"Get all feedbacks by party {token.Mail}");
            return Ok(new SuccessResponse<DynamicModelResponse<ServiceApplicationFeedBackViewModel>>((int)HttpStatusCode.OK, "Search success.", result));
        }

        [Authorize(Roles = "Admin, Location Owner, Service Provider")]
        [HttpGet("id")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetFeedbackById(Guid id)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _serviceApplicationFeedBackService.GetFeedbackById(id);
            _logger.LogInformation($"Get feedback by party {token.Mail}");
            return Ok(new SuccessResponse<ServiceApplicationFeedBackViewModel>((int)HttpStatusCode.OK, "Search success.", result));
        }
    }
}
