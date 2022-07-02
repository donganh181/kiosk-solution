
using kiosk_solution.Business.Services;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using kiosk_solution.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/testNoti")]
    [ApiController]
    [ApiVersion("1")]
    public class SendNotificationController : Controller
    {
        private readonly INotiService _notiService;
        private readonly ILogger<SendNotificationController> _logger;
        private IConfiguration _configuration;

        public SendNotificationController(INotiService notiService, ILogger<SendNotificationController> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _notiService = notiService;
            _configuration = configuration;
        }

        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> SendNoti([FromQuery] string deviceId)
        {
            return Ok(await _notiService.SendNotificationToUser(deviceId));
        }
    }
}
