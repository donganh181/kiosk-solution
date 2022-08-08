using System;
using System.Net;
using System.Threading.Tasks;
using kiosk_solution.Data.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kiosk_solution.Controllers
{ 
    [Route("api/v{version:apiVersion}/dashboard")]
    [ApiController]
    [ApiVersion("1")]
    public class DashBoardController : Controller
    {
        [HttpGet("/count/kiosk")]
        [MapToApiVersion("1")]
        [Authorize(Roles = "Admin, Location Owner")]
        public async Task<IActionResult> countKiosk()
        {
            return Ok();
        }
        [HttpGet("/count/app")]
        [MapToApiVersion("1")]
        [Authorize(Roles = "Admin, Location Owner")]
        public async Task<IActionResult> countApplication()
        {
            return Ok();
        }
        
        [HttpGet("/count/event")]
        [MapToApiVersion("1")]
        [Authorize(Roles = "Admin, Location Owner")]
        public async Task<IActionResult> countEvents()
        {
            return Ok();
        }
        [HttpGet("/count/poi")]
        [MapToApiVersion("1")]
        [Authorize(Roles = "Admin, Location Owner")]
        public async Task<IActionResult> countPOIs()
        {
            return Ok();
        }
        
        [HttpGet("/count/location-owner")]
        [MapToApiVersion("1")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> countLocationOwner()
        {
            return Ok();
        }
        [HttpGet("/count/service-provider")]
        [MapToApiVersion("1")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> countServiceProvider()
        {
            return Ok();
        }
    }
}