using kiosk_solution.Business.Services;
using kiosk_solution.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/locationOwners")]
    [ApiController]
    [ApiVersion("1")]
    public class LocationOwnerController : Controller
    {
        private readonly ILocationOwnerService _locationOwnerService;
        public LocationOwnerController(ILocationOwnerService locationOwnerService)
        {
            _locationOwnerService = locationOwnerService;
        }

        /// <summary>
        /// Login to LocationOwner account with email and password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel request)
        {
            return Ok(await _locationOwnerService.Login(request));

        }
    }
}
