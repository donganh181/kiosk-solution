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
    [Route("api/v{version:apiVersion}/kiosks")]
    [ApiController]
    [ApiVersion("1")]
    public class KioskController : Controller
    {
        private readonly IKioskService _kioskService;
        private readonly ILogger<KioskController> _logger;
        private IConfiguration _configuration;

        public KioskController(IKioskService kioskService, ILogger<KioskController> logger, IConfiguration configuration)
        {
            _kioskService = kioskService;
            _configuration = configuration;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("status")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateStatus(Guid id)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _kioskService.UpdateStatus(id);
            _logger.LogInformation($"Update status of kiosk [{result.Id}] by party {token.Mail}");
            return Ok(new SuccessResponse<KioskViewModel>((int) HttpStatusCode.OK, "Update success.", result));
        }
    }
}