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
        private readonly ICUGetResumenVentasMes _getResumenVentasMes;

        public VentasController(
            ICUGetResumenVentasDia getResumenVentasDia,
            ICUGetResumenVentasMes getResumenVentasMes)
        {
            _getResumenVentasDia = getResumenVentasDia;
            _getResumenVentasMes = getResumenVentasMes;
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

        [HttpGet("resumen-mes")]
        public IActionResult GetResumenMes([FromQuery] int? anio = null, [FromQuery] int? mes = null)
        {
            try
            {
                var now = DateTime.UtcNow;
                var anioTarget = anio ?? now.Year;
                var mesTarget = mes ?? now.Month;

                if (anioTarget <= 0 || mesTarget < 1 || mesTarget > 12)
                {
                    return BadRequest(new { error = "Los parametros anio/mes son invalidos." });
                }

                var resumen = _getResumenVentasMes.Execute(anioTarget, mesTarget);
                var dto = new VentaDtoResumenMes(
                    resumen.Anio,
                    resumen.Mes,
                    resumen.CantidadVentas,
                    resumen.TotalVendido);

                return Ok(dto);
            }
            catch (InfraestructuraException e)
            {
                return StatusCode(e.StatusCode(), new { error = e.Message });
            }
        }
    }
}
