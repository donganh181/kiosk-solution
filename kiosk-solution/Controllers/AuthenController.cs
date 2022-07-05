using kiosk_solution.Business.Services;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using kiosk_solution.Utils;
using System.Net;

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/auth")]
    [ApiController]
    [ApiVersion("1")]
    public class AuthenController : Controller
    {
        private readonly IPartyService _partyService;
        private readonly ILogger<AuthenController> _logger;
        private IConfiguration _configuration;
        public AuthenController(IPartyService partyService,ILogger<AuthenController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _partyService = partyService;
            _configuration = configuration;
        }
        
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel request)
        {
            var result = await _partyService.Login(request);
            _logger.LogInformation($"Login by {request.Email}");
            return Ok(new SuccessResponse<PartyViewModel>(200, "Login Success." , result));
        }

        [Authorize(Roles = "Admin, Location Owner, Service Provider")]
        [HttpPost("logout")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Logout()
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            _logger.LogInformation($"Logout by party {token.Mail}");
            return Ok(await _partyService.Logout(token.Id));
        }

    }
}
