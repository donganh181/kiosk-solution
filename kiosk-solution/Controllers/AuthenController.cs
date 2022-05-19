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

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/auth")]
    [ApiController]
    [ApiVersion("1")]
    public class AuthenController : Controller
    {
        private readonly IPartyService _partyService;
        private readonly ILogger<AuthenController> _logger;
        public AuthenController(IPartyService partyService,ILogger<AuthenController> logger)
        {
            _logger = logger;
            _partyService = partyService;
        }
        
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel request)
        {
            var result = await _partyService.Login(request);
            _logger.LogInformation($"Login by {request.email}");
            return Ok(new SuccessResponse<PartyViewModel>(200, "Login Success." , result));
        }

        
        
    }
}
