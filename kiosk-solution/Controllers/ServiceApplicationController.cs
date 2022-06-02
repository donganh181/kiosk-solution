using kiosk_solution.Business.Services;
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
        [HttpPut]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Update([FromBody] UpdateServiceApplicationViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            Guid updaterId = token.Id;
            var result = await _serviceApplicationService.UpdateInformation(updaterId, model);
            _logger.LogInformation($"Updated application {result.Name} by party {token.Mail}");
            return Ok(new SuccessResponse<ServiceApplicationViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }
    }
}
