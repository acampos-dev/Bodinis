using Bodinis.Infraestructura.AccesoDatos.Excepciones;
using Bodinis.LogicaAplicacion.DTOs.Caja;
using Bodinis.LogicaAplicacion.Interfaces;
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
        private readonly ICUAdd<CajaDtoAbrir> _abrirCaja;
        private readonly ICUCerrarCaja _cerrarCaja;
        private readonly ICUGetCajaActual _getCajaActual;

        public CajaController(
            ICUAdd<CajaDtoAbrir> abrirCaja,
            ICUCerrarCaja cerrarCaja,
            ICUGetCajaActual getCajaActual)
        {
            _abrirCaja = abrirCaja;
            _cerrarCaja = cerrarCaja;
            _getCajaActual = getCajaActual;
        }

        [HttpGet("actual")]
        public IActionResult GetActual()
        {
            try
            {
                return Ok(_getCajaActual.Execute());
            }
            catch (LogicaNegocioException e)
            {
                return BadRequest(new { error = e.Message });
            }
            catch (InfraestructuraException e)
            {
                return StatusCode(e.StatusCode(), new { error = e.Message });
            }
        }

        [HttpPost("abrir")]
        public IActionResult Abrir([FromBody] CajaDtoAbrir dto)
        {
            try
            {
                _abrirCaja.Execute(dto);
                return StatusCode(201);
            }
            catch (LogicaNegocioException e)
            {
                return BadRequest(new { error = e.Message });
            }
            catch (InfraestructuraException e)
            {
                return StatusCode(e.StatusCode(), new { error = e.Message });
            }
        }

        [HttpPost("cerrar")]
        public IActionResult Cerrar([FromBody] CajaDtoCerrar dto)
        {
            try
            {
                return Ok(_cerrarCaja.Execute(dto));
            }
            catch (LogicaNegocioException e)
            {
                return BadRequest(new { error = e.Message });
            }
            catch (InfraestructuraException e)
            {
                return StatusCode(e.StatusCode(), new { error = e.Message });
            }
        }
    }
}
