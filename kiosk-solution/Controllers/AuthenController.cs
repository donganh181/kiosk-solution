using kiosk_solution.Business.Services;
using kiosk_solution.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/auth")]
    [ApiController]
    [ApiVersion("1")]
    public class AuthenController : Controller
    {
        private readonly IPartyService _partyService;

        public AuthenController(IPartyService partyService)
        {
            _partyService = partyService;
        }

        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel request)
        {
            return Ok(await _partyService.Login(request));
        }

    }
}
