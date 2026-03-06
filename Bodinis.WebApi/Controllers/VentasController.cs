using Bodinis.Infraestructura.AccesoDatos.Excepciones;
using Bodinis.LogicaAplicacion.DTOs.Ventas;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bodinis.WebApi.Controllers
{
    [ApiController]
    [Route("api/ventas")]
    [Authorize]
    public class VentasController : ControllerBase
    {
        private readonly ICUGetResumenVentasDia _getResumenVentasDia;

        public VentasController(ICUGetResumenVentasDia getResumenVentasDia)
        {
            _getResumenVentasDia = getResumenVentasDia;
        }

        [HttpGet("resumen-dia")]
        public IActionResult GetResumenDia([FromQuery] DateOnly? fecha = null)
        {
            try
            {
                var fechaTarget = fecha ?? DateOnly.FromDateTime(DateTime.UtcNow);
                var resumen = _getResumenVentasDia.Execute(fechaTarget);
                var dto = new VentaDtoResumenPeriodo(
                    resumen.Fecha,
                    resumen.CantidadVentas,
                    resumen.TotalVendido,
                    resumen.TicketPromedio);

                return Ok(dto);
            }
            catch (InfraestructuraException e)
            {
                return StatusCode(e.StatusCode(), new { error = e.Message });
            }
        }
    }
}
