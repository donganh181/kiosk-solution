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
using System.Net;
using System.Threading.Tasks;

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/serviceApplications")]
    [ApiController]
    [ApiVersion("1")]
    public class ServiceApplicationController : Controller
    {
        private readonly IServiceApplicationService _serviceApplicationService;
        private readonly ILogger<ServiceApplicationController> _logger;
        private IConfiguration _configuration;
        public ServiceApplicationController(IServiceApplicationService serviceApplicationService, ILogger<ServiceApplicationController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _serviceApplicationService = serviceApplicationService;
            _configuration = configuration;
        }
        
        [Authorize(Roles = "Service Provider")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create([FromBody] CreateServiceApplicationViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _serviceApplicationService.Create(token.Id, model);
            _logger.LogInformation($"Create application {result.Name} by party {token.Mail}");
            return Ok(new SuccessResponse<ServiceApplicationViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }

        [Authorize(Roles = "Service Provider")]
        [HttpPatch("banner")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateBanner([FromBody] ServiceApplicationUpdateBannerViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _serviceApplicationService.UpdateBanner(token.Id, model);
            _logger.LogInformation($"Update app banner success by party {token.Mail}");
            return Ok(new SuccessResponse<ServiceApplicationViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }
        
        /// <summary>
        /// Update information by service provider
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Service Provider")]
        [HttpPut]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateInformation([FromBody] UpdateServiceApplicationViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            Guid updaterId = token.Id;
            var result = await _serviceApplicationService.UpdateInformation(updaterId, model);
            _logger.LogInformation($"Updated application {result.Name} by party {token.Mail}");
            return Ok(new SuccessResponse<ServiceApplicationViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// Search list app by admin or service provider
        /// </summary>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Get([FromQuery] ServiceApplicationSearchViewModel model, int size, int page = CommonConstants.DefaultPage)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            if(token == null)
            {
                var result = await _serviceApplicationService.GetAllWithPaging(null, null, model, size, page);
                _logger.LogInformation("Get all applications by guest");
                return Ok(new SuccessResponse<DynamicModelResponse<ServiceApplicationSearchViewModel>>((int)HttpStatusCode.OK, "Search success.", result));

            }
            else
            {
                Guid id = token.Id;
                string role = token.Role;
                var result = await _serviceApplicationService.GetAllWithPaging(role, id, model, size, page);
                _logger.LogInformation($"Get all applications by party {token.Mail}");
                return Ok(new SuccessResponse<DynamicModelResponse<ServiceApplicationSearchViewModel>>((int)HttpStatusCode.OK, "Search success.", result));

            }
        }
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAppById(Guid id)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            if (token == null)
            {
                var result = await _serviceApplicationService.GetById(null, id);
                _logger.LogInformation("Get application by guest");
                return Ok(new SuccessResponse<ServiceApplicationSpecificViewModel>((int)HttpStatusCode.OK, "Search success.", result));
            }
            else
            {
                var result = await _serviceApplicationService.GetById(token.Id, id);
                _logger.LogInformation("Get application by {token.Mail}");
                return Ok(new SuccessResponse<ServiceApplicationSpecificViewModel>((int)HttpStatusCode.OK, "Search success.", result));
            }
                
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPatch("status")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateStatus([FromBody] ServiceApplicationUpdateStatusViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _serviceApplicationService.UpdateStatus(model);
            _logger.LogInformation($"Stop application {result.Name} by party {token.Mail}");
            return Ok(new SuccessResponse<ServiceApplicationViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }
    }
}
