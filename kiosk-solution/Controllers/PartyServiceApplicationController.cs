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
    [Route("api/v{version:apiVersion}/my-app")]
    [ApiController]
    [ApiVersion("1")]
    public class PartyServiceApplicationController : Controller
    {
        private readonly IPartyServiceApplicationService _partyServiceApplicationService;
        private readonly ILogger<PartyServiceApplicationController> _logger;
        private IConfiguration _configuration;
        public PartyServiceApplicationController(IPartyServiceApplicationService partyServiceApplicationService, ILogger<PartyServiceApplicationController> logger, IConfiguration configuration)
        {
            _partyServiceApplicationService = partyServiceApplicationService;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Install app by location owner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Location Owner")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create([FromBody] PartyServiceApplicationCreateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _partyServiceApplicationService.Create(token.Id, model);
            _logger.LogInformation($"Get {result.ServiceApplicationName} by party {token.Mail}.");
            return Ok(new SuccessResponse<PartyServiceApplicationViewModel>((int)HttpStatusCode.OK, "Install app.", result));
        }

        [Authorize(Roles = "Location Owner")]
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Get([FromQuery] PartyServiceApplicationSearchViewModel model, int size, int page = CommonConstants.DefaultPage)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            Guid id = token.Id;
            var result = await _partyServiceApplicationService.GetAllWithPaging(id, model, size, page);
            _logger.LogInformation($"Get installed applications by party {token.Mail}");
            return Ok(new SuccessResponse<DynamicModelResponse<PartyServiceApplicationSearchViewModel>>((int)HttpStatusCode.OK, "Search success.", result));
        }
    }
}
