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
        public IActionResult Login([FromBody] LoginRequestDto dto)
        {
            try
            {
                var token = _login.Execute(dto);
                return Ok(new { token });
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


    }
}
