using Bodinis.Infraestructura.AccesoDatos.Excepciones;
using Bodinis.LogicaAplicacion.DTOs.Cajas;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bodinis.WebApi.Controllers
{
    [ApiController]
    [Route("api/caja")]
    [Authorize]
    public class CajaController : ControllerBase
    {
        private readonly ICUAbrirCaja _abrirCaja;
        private readonly ICUCerrarCaja _cerrarCaja;
        private readonly ICUGetCajaAbierta _getCajaAbierta;

        public CajaController(
            ICUAbrirCaja abrirCaja,
            ICUCerrarCaja cerrarCaja,
            ICUGetCajaAbierta getCajaAbierta)
        {
            _abrirCaja = abrirCaja;
            _cerrarCaja = cerrarCaja;
            _getCajaAbierta = getCajaAbierta;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("abrir")]
        public IActionResult Abrir([FromBody] CajaDtoApertura dto)
        {
            try
            {
                var resumen = _abrirCaja.Execute(dto.MontoApertura);
                return Ok(resumen);
            }
            catch (InfraestructuraException e)
            {
                return StatusCode(e.StatusCode(), new { error = e.Message });
            }
            catch (LogicaNegocioException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("cerrar")]
        public IActionResult Cerrar()
        {
            try
            {
                var resumen = _cerrarCaja.Execute();
                return Ok(resumen);
            }
            catch (InfraestructuraException e)
            {
                return StatusCode(e.StatusCode(), new { error = e.Message });
            }
            catch (LogicaNegocioException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpGet("abierta")]
        public IActionResult GetAbierta()
        {
            try
            {
                var resumen = _getCajaAbierta.Execute();
                return Ok(resumen);
            }
            catch (InfraestructuraException e)
            {
                return StatusCode(e.StatusCode(), new { error = e.Message });
            }
        }
    }
}
