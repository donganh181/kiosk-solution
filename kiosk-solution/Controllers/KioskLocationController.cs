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
    [Route("api/v{version:apiVersion}/kioskLocations")]
    [ApiController]
    [ApiVersion("1")]
    public class KioskLocationController : Controller
    {
        private readonly IKioskLocationService _kioskLocationService;
        private readonly ILogger<KioskLocationController> _logger;
        private IConfiguration _configuration;

        public KioskLocationController(IKioskLocationService kioskLocationService, 
            ILogger<KioskLocationController> logger, IConfiguration configuration)
        {
            _kioskLocationService = kioskLocationService;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Create new Kiosk Location by admin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CreateNewKioskLocation([FromBody] CreateKioskLocationViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _kioskLocationService.CreateNew(model);
            _logger.LogInformation($"Create new Kiosk Location by party {token.Mail}");
            return Ok(new SuccessResponse<KioskLocationViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }

        /// <summary>
        /// Search Location by admin
        /// </summary>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Get([FromQuery] KioskLocationSearchViewModel model, int size, int page = CommonConstants.DefaultPage)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            Guid id = token.Id;
            var result = await _kioskLocationService.GetAllWithPaging(model, size, page);
            _logger.LogInformation($"Get all Kiosk Locations by party {token.Mail}");
            return Ok(new SuccessResponse<DynamicModelResponse<KioskLocationSearchViewModel>>((int)HttpStatusCode.OK, "Search success.", result));
        }

        /// <summary>
        /// Update kiosk location by admin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateInformation([FromBody] UpdateKioskLocationViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _kioskLocationService.UpdateInformation(model);
            _logger.LogInformation($"Update kiosk location by party {token.Mail}");
            return Ok(new SuccessResponse<KioskLocationViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// Update status of kiosk location by admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPatch]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateStatus([FromQuery] Guid id)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _kioskLocationService.UpdateStatus(id);
            _logger.LogInformation($"Update status of kiosk location {result.Id} by party {token.Mail}");
            return Ok(new SuccessResponse<KioskLocationViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }
    }
}
