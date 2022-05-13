using kiosk_solution.Business.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using kiosk_solution.Data.ViewModels;
using kiosk_solution.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/partys")]
    [ApiController]
    [ApiVersion("1")]
    public class PartyController : Controller
    {
        private readonly IPartyService _partyService;
        private IConfiguration _configuration;
        public PartyController(IPartyService partyService,IConfiguration configuration)
        {
            _partyService = partyService;
            _configuration = configuration;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _partyService.GetAll());
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost("createAccount")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Register([FromBody] CreateAccountViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            Guid creatorId = token.Id;
            return Ok(await _partyService.CreateAccount(creatorId, model));
        }
    }
}