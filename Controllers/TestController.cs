using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PizzaStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminEndpoint()
        {
            return Ok("Hello Admin!");
        }

        [HttpGet("user")]
        [Authorize(Roles = "User")]
        public IActionResult UserEndpoint()
        {
            return Ok("Hello User!");
        }

        [HttpGet("all")]
        [Authorize]
        public IActionResult AnyAuthenticatedUser()
        {
            return Ok("Hello Authenticated User!");
        }
    }
}
