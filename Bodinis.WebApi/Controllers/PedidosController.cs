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
        private readonly ICUCrearPedido _crearPedido;
        private readonly ICUGetById<PedidoDtoTicket> _getPedidoTicketById;
        private readonly ICUGetResumenPedidos _getResumenPedidos;

        public PedidosController(
            ICUCrearPedido crearPedido,
            ICUGetById<PedidoDtoTicket> getPedidoTicketById,
            ICUGetResumenPedidos getResumenPedidos)
        {
            _crearPedido = crearPedido;
            _getPedidoTicketById = getPedidoTicketById;
            _getResumenPedidos = getResumenPedidos;
        }

        [HttpPost]
        public IActionResult Add([FromBody] PedidoDtoCrear dto)
        {
            try
            {
                var ticket = _crearPedido.Execute(dto);
                return CreatedAtAction(nameof(GetTicketById), new { id = ticket.PedidoId }, ticket);
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

        [HttpGet("{id:int}/ticket")]
        public IActionResult GetTicketById(int id)
        {
            try
            {
                var ticket = _getPedidoTicketById.Execute(id);
                return Ok(ticket);
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

        [HttpGet("resumen")]
        public IActionResult GetResumen([FromQuery] DateOnly desde, [FromQuery] DateOnly hasta)
        {
            try
            {
                if (hasta < desde)
                {
                    return BadRequest(new { error = "El rango de fechas es inválido." });
                }

                var resumen = _getResumenPedidos.Execute(desde, hasta);
                return Ok(resumen);
            }
            catch (InfraestructuraException e)
            {
                return StatusCode(e.StatusCode(), new { error = e.Message });
            }
        }
    }
}
