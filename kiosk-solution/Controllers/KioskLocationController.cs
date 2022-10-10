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
        /// Create new Kiosk Location by location owner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Location Owner")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CreateNewKioskLocation([FromBody] CreateKioskLocationViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _kioskLocationService.CreateNew(token.Id, model);
            _logger.LogInformation($"Create new Kiosk Location by party {token.Mail}");
            return Ok(new SuccessResponse<KioskLocationViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }

        /// <summary>
        /// Search Location by location owner
        /// </summary>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Authorize(Roles = "Location Owner")]
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Get([FromQuery] KioskLocationSearchViewModel model, int size, int page = CommonConstants.DefaultPage)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            Guid id = token.Id;
            var result = await _kioskLocationService.GetAllWithPaging(token.Id, model, size, page);
            _logger.LogInformation($"Get all Kiosk Locations by party {token.Mail}");
            return Ok(new SuccessResponse<DynamicModelResponse<KioskLocationSearchViewModel>>((int)HttpStatusCode.OK, "Search success.", result));
        }

        /// <summary>
        /// Update kiosk location by location owner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Location Owner")]
        [HttpPut]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateInformation([FromBody] UpdateKioskLocationViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _kioskLocationService.UpdateInformation(token.Id, model);
            _logger.LogInformation($"Update kiosk location by party {token.Mail}");
            return Ok(new SuccessResponse<KioskLocationViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// Replace image by its location owner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Location Owner")]
        [HttpPut("replace")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ReplaceImage([FromBody] ImageReplaceViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _kioskLocationService.ReplaceImage(token.Id, model);
            _logger.LogInformation($"Update event images success by party {token.Mail}");
            return Ok(new SuccessResponse<KioskLocationViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// Get location by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("id")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Get([FromQuery] Guid id, bool isNotDes)
        {
            var result = await _kioskLocationService.GetById(id, isNotDes);
            _logger.LogInformation($"Get information of location {result.Name} by guest");
            return Ok(new SuccessResponse<KioskLocationViewModel>((int)HttpStatusCode.OK, "Search success.", result));
        }

        [HttpGet("async-information")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> AsyncInformation([FromQuery] Guid id)
        {
            var result = await _kioskLocationService.GetByIdAndChangeKioskView(id);
            return Ok(new SuccessResponse<KioskLocationViewModel>((int)HttpStatusCode.OK, "Async success.", result));
        }
    }
}
