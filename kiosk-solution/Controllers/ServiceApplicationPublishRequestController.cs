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
    [Route("api/v{version:apiVersion}/publishRequests")]
    [ApiController]
    [ApiVersion("1")]
    public class ServiceApplicationPublishRequestController : Controller
    {
        private readonly IServiceApplicationPublishRequestService _requestPublishService;
        private readonly ILogger<ServiceApplicationPublishRequestController> _logger;
        private IConfiguration _configuration;

        public ServiceApplicationPublishRequestController(
            IServiceApplicationPublishRequestService requestPublishService,
            ILogger<ServiceApplicationPublishRequestController> logger, IConfiguration configuration)
        {
            _requestPublishService = requestPublishService;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Create publish request by service provider
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Service Provider")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create([FromBody] ServiceApplicationPublishRequestCreateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _requestPublishService.Create(token.Id, model);
            _logger.LogInformation($"Create publish request by party {token.Mail}");
            return Ok(new SuccessResponse<ServiceApplicationPublishRequestViewModel>((int) HttpStatusCode.OK,
                "Create success.", result));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Update([FromBody] UpdateServiceApplicationPublishRequestViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _requestPublishService.Update(token.Id, model);
            _logger.LogInformation($"Update publish request by party {token.Mail}");
            return Ok(new SuccessResponse<ServiceApplicationPublishRequestViewModel>((int) HttpStatusCode.OK,
                "Update success.", result));
        }

        /// <summary>
        /// Get all publish request by admin and service provider
        /// </summary>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Service Provider")]
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Get([FromQuery] ServiceApplicationPublishRequestSearchViewModel model,
            int size, int page = CommonConstants.DefaultPage)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            Guid id = token.Id;
            string role = token.Role;
            var result = await _requestPublishService.GetAllWithPaging(role, id, model, size, page);
            _logger.LogInformation($"Get all Kiosks by party {token.Mail}");
            return Ok(new SuccessResponse<DynamicModelResponse<ServiceApplicationPublishRequestSearchViewModel>>(
                (int) HttpStatusCode.OK, "Search success.", result));
        }

        /// <summary>
        /// Admin and their own service provider can get request publish by id.
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Service Provider")]
        [HttpGet("id")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetById(Guid requestId)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _requestPublishService.GetById(token.Id, token.Role, requestId);
            _logger.LogInformation($"Get request by id {requestId}");
            return Ok(new SuccessResponse<ServiceApplicationPublishRequestViewModel>((int) HttpStatusCode.OK,
                "Get success.", result));
        }

        [Authorize(Roles = "Admin, Service Provider")]
        [HttpGet("inprogress/appId/{appId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetInProgressByAppId(Guid appId)
        {
            var result = await _requestPublishService.GetInprogressByAppId(appId);
            return Ok(new SuccessResponse<ServiceApplicationPublishRequestViewModel>((int) HttpStatusCode.OK,
                "Get success.", result));
        }

        [Authorize(Roles = "Service Provider")]
        [HttpPatch("status")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CancelRequest([FromBody] Guid ticketId)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _requestPublishService.UpdateStatusByOwner(token.Id, ticketId);
            _logger.LogInformation($"Cancel publish request by party {token.Mail}");
            return Ok(new SuccessResponse<ServiceApplicationPublishRequestViewModel>((int)HttpStatusCode.OK,
                "Cancel success.", result));
        }
    }
}