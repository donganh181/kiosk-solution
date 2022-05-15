using kiosk_solution.Business.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using kiosk_solution.Data.ViewModels;
using kiosk_solution.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using kiosk_solution.Data.Responses;
using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Logging;

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/parties")]
    [ApiController]
    [ApiVersion("1")]
    public class PartyController : Controller
    {
        private readonly IPartyService _partyService;
        private readonly ILogger<PartyController> _logger;
        private IConfiguration _configuration;
        public PartyController(IPartyService partyService,IConfiguration configuration, ILogger<PartyController> logger)
        {
            _partyService = partyService;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Get all users in system
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _partyService.GetAll();
            return Ok(new SuccessResponse<List<PartyViewModel>> ((int)HttpStatusCode.OK, "Found.", list));
        }
        
        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Register([FromBody] CreateAccountViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            Guid creatorId = token.Id;
            var result = await _partyService.CreateAccount(creatorId, model);
            _logger.LogInformation($"Created party {result.Email} by party {token.Mail}");
            return Ok(new SuccessResponse<PartyViewModel>((int)HttpStatusCode.OK, "Create success", result));
        }
    }
}