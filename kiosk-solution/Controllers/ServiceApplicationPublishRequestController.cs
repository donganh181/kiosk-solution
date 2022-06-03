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
    [Route("api/v{version:apiVersion}/publishRequests")]
    [ApiController]
    [ApiVersion("1")]
    public class ServiceApplicationPublishRequestController : Controller
    {
        private readonly IServiceApplicationPublishRequestService _requestPublishService;
        private readonly ILogger<ServiceApplicationPublishRequestController> _logger;
        private IConfiguration _configuration;
        public ServiceApplicationPublishRequestController(IServiceApplicationPublishRequestService requestPublishService, ILogger<ServiceApplicationPublishRequestController> logger, IConfiguration configuration)
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
            return Ok(new SuccessResponse<ServiceApplicationPublishRequestViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }
    }
}
