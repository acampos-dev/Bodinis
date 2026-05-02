using Bodinis.LogicaAplicacion.DTOs;
using Bodinis.LogicaAplicacion.DTOs.Usuarios;
using Bodinis.LogicaAplicacion.Interfaces;
using Bodinis.LogicaNegocio.Excepciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bodinis.WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogin<LoginDtoRequest, LoginDtoResponse> _login;

        public AuthController(ILogin<LoginDtoRequest, LoginDtoResponse> login)
        {
            _login = login;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDtoRequest dto)
        {
            try
            {
                var response = _login.Execute(dto);
                return Ok(response);
            }
            catch (CredencialesInvalidasException e)
            {
                return Unauthorized(new { error = e.Message });
            }
            catch (UsuarioInactivoException e)
            {
                return StatusCode(403, new { error = e.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { message = "Logout exitoso. El cliente debe eliminar el token JWT." });
        }


    }
}
