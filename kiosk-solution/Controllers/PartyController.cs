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
using kiosk_solution.Data.Constants;

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

        public PartyController(IPartyService partyService, IConfiguration configuration,
            ILogger<PartyController> logger)
        {
            _partyService = partyService;
            _configuration = configuration;
            _logger = logger;
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
            return Ok(new SuccessResponse<PartyViewModel>((int) HttpStatusCode.OK, "Create success.", result));
        }

        /// <summary>
        /// Admin can update information of other users. Other can update their own information.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Location Owner, Service Provider")]
        [HttpPut]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Update([FromBody] UpdateAccountViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            Guid updaterId = token.Id;
            var result = await _partyService.UpdateAccount(updaterId, model);
            _logger.LogInformation($"Updated party {result.Email} by party {token.Mail}");
            return Ok(new SuccessResponse<PartyViewModel>((int) HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// User can update their own password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Location Owner, Service Provider")]
        [HttpPatch("password")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            Guid id = token.Id;
            var result = await _partyService.UpdatePassword(id, model);
            _logger.LogInformation($"Updated password of party {result.Email}");
            return Ok(new SuccessResponse<PartyViewModel>((int) HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// Admin can change the status of user account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPatch("status")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateStatus(Guid id)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _partyService.UpdateStatus(id);
            _logger.LogInformation($"Updated status of party {result.Email} by party {token.Mail}");
            return Ok(new SuccessResponse<PartyViewModel>((int) HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// Search user
        /// </summary>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Get([FromQuery] PartySearchViewModel model, int size,
            int page = CommonConstants.DefaultPage)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            Guid id = token.Id;
            var result = await _partyService.GetAllWithPaging(model, size, page);
            _logger.LogInformation($"Get all parties by party {token.Mail}");
            return Ok(new SuccessResponse<DynamicModelResponse<PartySearchViewModel>>((int) HttpStatusCode.OK,
                "Search success.", result));
        }
        [Authorize(Roles = "Location Owner, Admin, Service Provider")]
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetPartyById(Guid id)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _partyService.GetPartyById(id, token.Role, token.Id);
            _logger.LogInformation($"Get all parties by party {token.Mail}");
            return Ok(new SuccessResponse<PartyViewModel>((int) HttpStatusCode.OK, "Search success.", result));
        }

        [HttpGet("kioskId")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetPartyIdByKioskId(Guid kioskId)
        {
            var result = await _partyService.GetPartyByKioskId(kioskId);
            _logger.LogInformation($"Get party by kioskId success");
            return Ok(new SuccessResponse<PartyByKioskIdViewModel>((int) HttpStatusCode.OK, "Search success.", result));
        }

        [HttpGet("forgetPassword")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ForgetPassword([FromQuery] string email)
        {
            var result = await _partyService.ForgetPassword(email);
            _logger.LogInformation($"Send forget password mail to party {email}");
            return Ok(result);
        }

        [HttpGet("resetPassword")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ResetPassword([FromQuery] Guid partyId, string verifyCode)
        {
            var result = await _partyService.ResetPassword(partyId, verifyCode);
            _logger.LogInformation($"Reset password by party {partyId}");
            return Ok(new SuccessResponse<PartyResetPasswordViewModel>((int) HttpStatusCode.OK, "Reset success.",
                result));
        }
    }
}