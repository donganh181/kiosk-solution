using System;
using System.Net;
using System.Threading.Tasks;
using kiosk_solution.Business.Services;
using kiosk_solution.Data.Constants;
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

        /// <summary>
        /// Update status Kiosk by its location owner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Location Owner")]
        [HttpPatch("status")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateStatus(Guid id)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _kioskService.UpdateStatus(token.Id,id);
            _logger.LogInformation($"Update status of kiosk [{result.Id}] by party {token.Mail}");
            return Ok(new SuccessResponse<KioskViewModel>((int) HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// Add device id into kiosk
        /// </summary>
        /// <param name="kioskId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpPatch("deviceId")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> AddDeviceId([FromBody] KioskAddDeviceIdViewModel model)
        {
            var result = await _kioskService.AddDeviceId(model);
            _logger.LogInformation($"Add DeviceId of kiosk [{result.Id}]");
            return Ok(new SuccessResponse<KioskViewModel>((int)HttpStatusCode.OK, "Add success.", result));
        }

        /// <summary>
        /// Create new Kiosk
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CreateNewKiosk([FromBody] CreateKioskViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _kioskService.CreateNewKiosk(model);
            _logger.LogInformation($"Create new Kiosk to [{result.PartyId}] by party {token.Mail}");
            return Ok(new SuccessResponse<KioskViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }

        /// <summary>
        /// Update information of kiosk by its location owner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Location Owner")]
        [HttpPut]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Update([FromBody] UpdateKioskViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            Guid updaterId = token.Id;
            var result = await _kioskService.UpdateInformation(updaterId, model);
            _logger.LogInformation($"Updated kiosk {result.Name} by party {token.Mail}");
            return Ok(new SuccessResponse<KioskViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// Search Kiosk by admin or location owner(only kiosk that belong to them)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Location Owner")]
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Get([FromQuery] KioskSearchViewModel model, int size, int page = CommonConstants.DefaultPage)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            Guid id = token.Id;
            string role = token.Role;
            var result = await _kioskService.GetAllWithPaging(role, id, model, size, page);
            _logger.LogInformation($"Get all Kiosks by party {token.Mail}");
            return Ok(new SuccessResponse<DynamicModelResponse<KioskSearchViewModel>>((int)HttpStatusCode.OK, "Search success.", result));
        }
    }
}