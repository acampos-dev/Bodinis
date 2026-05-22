using Bodinis.Infraestructura.AccesoDatos.Excepciones;
using Bodinis.LogicaAplicacion.DTOs.Ventas;
using Bodinis.LogicaNegocio.Excepciones;
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
        private readonly ICUAdd<VentaDtoAlta> _registrarVenta;
        private readonly ICUGetAll<VentaDtoListado> _getAllVentas;
        private readonly ICUGetById<VentaDtoListado> _getVentaById;

        public VentasController(
            ICUAdd<VentaDtoAlta> registrarVenta,
            ICUGetAll<VentaDtoListado> getAllVentas,
            ICUGetById<VentaDtoListado> getVentaById)
        {
            _registrarVenta = registrarVenta;
            _getAllVentas = getAllVentas;
            _getVentaById = getVentaById;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_getAllVentas.Execute());
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_getVentaById.Execute(id));
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
        public IActionResult Registrar([FromBody] VentaDtoAlta dto)
        {
            try
            {
                _registrarVenta.Execute(dto);
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
