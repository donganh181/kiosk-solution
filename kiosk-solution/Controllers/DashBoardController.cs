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
    [Route("api/v{version:apiVersion}/dashboard")]
    [ApiController]
    [ApiVersion("1")]
    public class DashBoardController : Controller
    {
        private readonly IKioskService _kioskService;
        private readonly IServiceApplicationService _applicationService;
        private readonly IEventService _eventService;
        private readonly IPoiService _poiService;
        private readonly IPartyService _partyService;
        private readonly ILogger _logger;
        private IConfiguration _configuration;

        public DashBoardController(
            IKioskService kioskService,
            IServiceApplicationService applicationService,
            IEventService eventService,
            IPoiService poiService,
            IPartyService partyService,
            ILogger<EventPositionController> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _kioskService = kioskService;
            _configuration = configuration;
            _applicationService = applicationService;
            _eventService = eventService;
            _poiService = poiService;
            _partyService = partyService;
        }

        [HttpGet("count/kiosk")]
        [MapToApiVersion("1")]
        [Authorize(Roles = "Admin, Location Owner")]
        public async Task<IActionResult> countKiosk()
        {
            var request = Request;
            var token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _kioskService.CountKiosks(token.Id, token.Role);
            return Ok(new SuccessResponse<CountViewModel>((int) HttpStatusCode.OK, "Get success.", result));
        }

        [HttpGet("count/app")]
        [MapToApiVersion("1")]
        [Authorize(Roles = "Admin, Service Provider, Location Owner")]
        public async Task<IActionResult> countApplication()
        {
            var request = Request;
            var token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _applicationService.CountApps(token.Id, token.Role);
            return Ok(new SuccessResponse<CountViewModel>((int) HttpStatusCode.OK, "Get success.", result));
        }

        [HttpGet("count/event")]
        [MapToApiVersion("1")]
        [Authorize(Roles = "Admin, Location Owner")]
        public async Task<IActionResult> countEvents()
        {
            var request = Request;
            var token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _eventService.CountEvents(token.Id, token.Role);
            return Ok(new SuccessResponse<CountViewModel>((int) HttpStatusCode.OK, "Get success.", result));
        }

        [HttpGet("count/poi")]
        [MapToApiVersion("1")]
        [Authorize(Roles = "Admin, Location Owner")]
        public async Task<IActionResult> countPOIs()
        {
            var request = Request;
            var token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _poiService.CountPOIs(token.Id, token.Role);
            return Ok(new SuccessResponse<CountViewModel>((int) HttpStatusCode.OK, "Get success.", result));
        }

        [HttpGet("count/location-owner")]
        [MapToApiVersion("1")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> countLocationOwner()
        {
            var result = await _partyService.CountLocationOwner();
            return Ok(new SuccessResponse<CountViewModel>((int) HttpStatusCode.OK, "Get success.", result));
        }

        [HttpGet("count/service-provider")]
        [MapToApiVersion("1")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> countServiceProvider()
        {
            var result = await _partyService.CountServiceProvider();
            return Ok(new SuccessResponse<CountViewModel>((int) HttpStatusCode.OK, "Get success.", result));
        }
    }
}