using kiosk_solution.Business.Services;
using kiosk_solution.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/testSendNoti")]
    [ApiController]
    [ApiVersion("1")]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(INotificationService notificationService, ILogger<NotificationController> logger)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CreateNotification([FromBody] NotificationCreateViewModel model )
        {
            return Ok(await _notificationService.Create(model));
        }
    }
}
