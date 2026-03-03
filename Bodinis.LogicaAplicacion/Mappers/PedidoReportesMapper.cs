using Bodinis.LogicaAplicacion.DTOs.Pedidos;    
using Bodinis.LogicaNegocio.Entidades;
using System.Linq;

namespace Bodinis.LogicaAplicacion.Mappers
{
    public static class PedidoReportesMapper
    {
        public static PedidoDtoResumenPeriodo ToResumenPeriodoDto(
            DateOnly fechaDesde,
            DateOnly fechaHasta,
            int cantidadPedidos,
            int totalFacturado,
            int cantidadDelivery,
            int cantidadRetiro)
        {
            int ticketPromedio = cantidadPedidos > 0 ? totalFacturado / cantidadPedidos : 0;

            return new PedidoDtoResumenPeriodo(
                fechaDesde,
                fechaHasta,
                cantidadPedidos,
                totalFacturado,
                ticketPromedio,
                cantidadDelivery,
                cantidadRetiro);
        }

        public static PedidoDtoTicket ToTicketDto(Pedido pedido, string cajero)
        {
            var items = pedido.Detalles
                .Select(ToTicketItemDto)
                .ToList();

            int subtotal = items.Sum(i => i.Subtotal);

            return new PedidoDtoTicket(
                pedido.Id,
                pedido.FechaHora,
                pedido.TipoPedido.ToString(),
                pedido.Estado.ToString(),
                cajero,
                items,
                subtotal,
                pedido.Total);
        }

        public static PedidoDtoTicketItem ToTicketItemDto(DetallePedido detalle)
        {
            return new PedidoDtoTicketItem(
                detalle.Producto.NombreProducto.Valor,
                detalle.Cantidad,
                detalle.PrecioUnitario,
                detalle.Subtotal);
        }
    }
}
