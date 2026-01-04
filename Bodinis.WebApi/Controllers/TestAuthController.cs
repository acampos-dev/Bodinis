using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bodinis.WebApi.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestAuthController: ControllerBase
    {
        [HttpGet("libre")]
        public IActionResult Libre()
        {
            return Ok("Endpoint libre");
        }

        [Authorize]
        [HttpGet("protegido")]
        public IActionResult Protegido()
        {
            return Ok("Estas autenticado");
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("solo-admin")]
        public IActionResult SoloAdmin()
        {
            return Ok("Eres admin");
        }
    }
}
