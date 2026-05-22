using Bodinis.Infraestructura.AccesoDatos.Excepciones;
using Bodinis.LogicaAplicacion.DTOs.Pedidos;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bodinis.WebApi.Controllers
{
    [ApiController]
    [Route("api/pedidos")]
    [Authorize]
    public class PedidosController : ControllerBase
    {
        private readonly ICUAdd<PedidoDtoAlta> _addPedido;
        private readonly ICUGetAll<PedidoDtoListado> _getAllPedidos;
        private readonly ICUGetById<PedidoDtoListado> _getPedidoById;
        private readonly ICUCambiarEstadoPedido _cambiarEstadoPedido;

        public PedidosController(
            ICUAdd<PedidoDtoAlta> addPedido,
            ICUGetAll<PedidoDtoListado> getAllPedidos,
            ICUGetById<PedidoDtoListado> getPedidoById,
            ICUCambiarEstadoPedido cambiarEstadoPedido)
        {
            _addPedido = addPedido;
            _getAllPedidos = getAllPedidos;
            _getPedidoById = getPedidoById;
            _cambiarEstadoPedido = cambiarEstadoPedido;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_getAllPedidos.Execute());
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_getPedidoById.Execute(id));
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
        public IActionResult Add([FromBody] PedidoDtoAlta dto)
        {
            try
            {
                _addPedido.Execute(dto);
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

        [HttpPatch("{id:int}/estado")]
        public IActionResult CambiarEstado(int id, [FromBody] PedidoDtoCambiarEstado dto)
        {
            try
            {
                _cambiarEstadoPedido.Execute(id, dto.Estado);
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
    }
}
