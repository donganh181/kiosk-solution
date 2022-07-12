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
    [Route("api/v{version:apiVersion}/partyNotifications")]
    [ApiController]
    [ApiVersion("1")]
    public class PartyNotificationController : Controller
    {
        private readonly IPartyNotificationService _partyNotificationService;
        private readonly ILogger<PartyNotificationController> _logger;
        private IConfiguration _configuration;

        public PartyNotificationController(IPartyNotificationService partyNotificationService,
            IConfiguration configuration, ILogger<PartyNotificationController> logger)
        {
            _partyNotificationService = partyNotificationService;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Get all notification
        /// </summary>
        /// <param name="size"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Location Owner, Service Provider")]
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Get([FromQuery] int size, int page = CommonConstants.DefaultPage)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            Guid id = token.Id;
            var result = await _partyNotificationService.Get(id, size, page);
            _logger.LogInformation($"Get all notifications by party {token.Mail}");
            return Ok(new SuccessResponse<NotificationDynamicModelResponse<PartyNotificationViewModel>>((int)HttpStatusCode.OK, "Search success.", result));
        }

        /// <summary>
        /// Get notification by id 
        /// </summary>
        /// <param name="partyNotiId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Location Owner, Service Provider")]
        [HttpPatch]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetById([FromBody] Guid partyNotiId)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            Guid partyId = token.Id;
            var result = await _partyNotificationService.GetById(partyId, partyNotiId) ;
            _logger.LogInformation($"Get notification by party {token.Mail}");
            return Ok(new SuccessResponse<PartyNotificationViewModel>((int)HttpStatusCode.OK, "Search success.", result));
        }
    }
}
