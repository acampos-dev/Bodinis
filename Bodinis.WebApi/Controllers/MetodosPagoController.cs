using Bodinis.Infraestructura.AccesoDatos.Excepciones;
using Bodinis.LogicaAplicacion.DTOs.MetodoPago;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bodinis.WebApi.Controllers
{
    [ApiController]
    [Route("api/metodos-pago")]
    [Authorize]
    public class MetodosPagoController : ControllerBase
    {
        private readonly ICUAdd<MetodoPagoDtoAlta> _addMetodoPago;
        private readonly ICUGetAll<MetodoPagoDtoListado> _getAllMetodosPago;
        private readonly ICUGetById<MetodoPagoDtoListado> _getMetodoPagoById;
        private readonly ICUUpdate<MetodoPagoDtoModificar> _updateMetodoPago;
        private readonly ICUDelete<MetodoPagoDtoListado> _deleteMetodoPago;

        public MetodosPagoController(
            ICUAdd<MetodoPagoDtoAlta> addMetodoPago,
            ICUGetAll<MetodoPagoDtoListado> getAllMetodosPago,
            ICUGetById<MetodoPagoDtoListado> getMetodoPagoById,
            ICUUpdate<MetodoPagoDtoModificar> updateMetodoPago,
            ICUDelete<MetodoPagoDtoListado> deleteMetodoPago)
        {
            _addMetodoPago = addMetodoPago;
            _getAllMetodosPago = getAllMetodosPago;
            _getMetodoPagoById = getMetodoPagoById;
            _updateMetodoPago = updateMetodoPago;
            _deleteMetodoPago = deleteMetodoPago;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_getAllMetodosPago.Execute());
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_getMetodoPagoById.Execute(id));
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Add([FromBody] MetodoPagoDtoAlta dto)
        {
            try
            {
                _addMetodoPago.Execute(dto);
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

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] MetodoPagoDtoModificar dto)
        {
            try
            {
                _updateMetodoPago.Execute(id, dto);
                return NoContent();
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

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _deleteMetodoPago.Execute(id);
                return NoContent();
            }
            catch (InfraestructuraException e)
            {
                return StatusCode(e.StatusCode(), new { error = e.Message });
            }
        }
    }
}
