﻿using kiosk_solution.Business.Services;
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
    [Route("api/v{version:apiVersion}/events")]
    [ApiController]
    [ApiVersion("1")]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly ILogger<EventController> _logger;
        private IConfiguration _configuration;
        public EventController(IEventService eventService, ILogger<EventController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _eventService = eventService;
            _configuration = configuration;
        }

        /// <summary>
        /// Create event by admin (Server event) or location owner (local event)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Location Owner")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create([FromBody] EventCreateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _eventService.Create(token.Id,token.Role, model);
            _logger.LogInformation($"Create event {result.Name} by party {token.Mail}");
            return Ok(new SuccessResponse<EventViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }
    }
}