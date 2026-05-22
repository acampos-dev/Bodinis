using Bodinis.Infraestructura.AccesoDatos.Excepciones;
using Bodinis.LogicaAplicacion.DTOs.Gastos;
using Bodinis.LogicaAplicacion.Interfaces;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bodinis.WebApi.Controllers
{
    [ApiController]
    [Route("api/gastos")]
    [Authorize]
    public class GastosController : ControllerBase
    {
        private readonly ICUAdd<GastoDtoAlta> _registrarGasto;
        private readonly ICUGetGastosPorCajaActual _getGastosCajaActual;

        public GastosController(
            ICUAdd<GastoDtoAlta> registrarGasto,
            ICUGetGastosPorCajaActual getGastosCajaActual)
        {
            _registrarGasto = registrarGasto;
            _getGastosCajaActual = getGastosCajaActual;
        }

        [HttpGet("caja-actual")]
        public IActionResult GetCajaActual()
        {
            try
            {
                return Ok(_getGastosCajaActual.Execute());
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

        [HttpPost]
        public IActionResult Registrar([FromBody] GastoDtoAlta dto)
        {
            try
            {
                _registrarGasto.Execute(dto);
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
    }
}
