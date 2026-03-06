using Bodinis.Infraestructura.AccesoDatos.Excepciones;
using Bodinis.LogicaAplicacion.DTOs.Pedidos;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.Enums;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.ModelosCasosUso;
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
        private readonly ICUGetPedidoById _getPedidoById;
        private readonly ICUGetResumenPedidos _getResumenPedidos;

        public PedidosController(
            ICUCrearPedido crearPedido,
            ICUGetPedidoById getPedidoById,
            ICUGetResumenPedidos getResumenPedidos)
        {
            _crearPedido = crearPedido;
            _getPedidoById = getPedidoById;
            _getResumenPedidos = getResumenPedidos;
        }

        [HttpPost]
        public IActionResult Add([FromBody] PedidoDtoCrear dto)
        {
            try
            {
                if (!Enum.TryParse<TipoPedido>(dto.TipoPedido, true, out var tipoPedido))
                {
                    return BadRequest(new { error = "Tipo de pedido inválido." });
                }

                var items = dto.Items.Select(i => new PedidoItemInput(i.ProductoId, i.Cantidad));
                var id = _crearPedido.Execute(dto.UsuarioId, tipoPedido, items);

                var pedido = _getPedidoById.Execute(id);
                var ticket = PedidoReportesMapper.ToTicketDto(pedido, pedido.Usuario.NombreCompleto);

                return CreatedAtAction(nameof(GetTicketById), new { id }, ticket);
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
                var pedido = _getPedidoById.Execute(id);
                var ticket = PedidoReportesMapper.ToTicketDto(pedido, pedido.Usuario.NombreCompleto);
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
                var dto = new PedidoDtoResumenPeriodo(
                    resumen.FechaDesde,
                    resumen.FechaHasta,
                    resumen.CantidadPedidos,
                    resumen.TotalFacturado,
                    resumen.TicketPromedio,
                    resumen.CantidadDelivery,
                    resumen.CantidadRetiro);

                return Ok(dto);
            }
            catch (InfraestructuraException e)
            {
                return StatusCode(e.StatusCode(), new { error = e.Message });
            }
        }
    }
}
