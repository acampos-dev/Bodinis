using Bodinis.Infraestructura.AccesoDatos.Excepciones;
using Bodinis.LogicaAplicacion.DTOs.Cajas;
using Bodinis.LogicaAplicacion.Mappers;
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
                var caja = _abrirCaja.Execute(dto.MontoApertura);
                return Ok(CajaReportesMapper.ToResumenDto(caja));
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
                var caja = _cerrarCaja.Execute();
                return Ok(CajaReportesMapper.ToResumenDto(caja, caja.FechaCierre));
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
                var caja = _getCajaAbierta.Execute();
                return Ok(CajaReportesMapper.ToResumenDto(caja));
            }
            catch (InfraestructuraException e)
            {
                return StatusCode(e.StatusCode(), new { error = e.Message });
            }
        }
    }
}
