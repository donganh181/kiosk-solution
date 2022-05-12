using kiosk_solution.Business.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using kiosk_solution.Utils;
using Microsoft.Extensions.Configuration;
using kiosk_solution.Data.ViewModels;
using kiosk_solution.Data.Constants;
using System.Net;
using kiosk_solution.Data.Responses;

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/partys")]
    [ApiController]
    [ApiVersion("1")]
    public class PartyController : Controller
    {
        private readonly IPartyService _partyService;
        private readonly IConfiguration _configuration;

        public PartyController(IPartyService partyService, IConfiguration configuration)
        {
            _partyService = partyService;
            _configuration = configuration;
        }

        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAll()
        {
            var tokenModel = HttpContextUtil.getTokenModelFromRequest(Request, _configuration);
            if (!tokenModel.Role.Equals(RoleConstants.ADMIN))
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "Your role cannot use this feature.");

            return Ok(await _partyService.GetAll());
        }
    }
}