using System.Threading.Tasks;
using kiosk_solution.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/roles")]
    [ApiController]
    [ApiVersion("1")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private IConfiguration _configuration;
        public RoleController(IRoleService roleService, IConfiguration configuration)
        {
            _roleService = roleService;
            _configuration = configuration;
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _roleService.GetAll());
        }
    }
}