using Bodinis.LogicaAplicacion.DTOs;
using Bodinis.LogicaAplicacion.DTOs.Usuarios;
using Bodinis.LogicaAplicacion.Interfaces;
using Bodinis.LogicaNegocio.Excepciones;
using Microsoft.AspNetCore.Mvc;

namespace Bodinis.WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController: ControllerBase
    {
        private readonly ILogin<LoginRequestDto> _login;

        public AuthController(ILogin<LoginRequestDto> login)
        {
            _login = login;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            try
            {
                var token = _login.Execute(request);
                return Ok(token);
            }
            catch (UsuarioInactivoException e)
            {
                return Unauthorized(e.Message);
            }
            catch (CredencialesInvalidasException e)
            {
                return Unauthorized(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

    }
}
