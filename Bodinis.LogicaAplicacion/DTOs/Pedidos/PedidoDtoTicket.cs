

namespace Bodinis.LogicaAplicacion.DTOs.Pedidos
{
    public record  PedidoDtoTicket(
                                     int PedidoId,
                                     DateTime FechaHora,
                                     string TipoPedido,
                                     string Estado,
                                     string Cajero,
                                     IReadOnlyCollection<PedidoDtoTicketItem> Items,
                                     int Subtotal,
                                     int Total
                                   )
    {
    }
    public record PedidoDtoTicketItem(
                                      string ProductoNombre,
                                      int Cantidad,
                                      int PrecioUnitario,
                                      int Subtotal
                                    )
    { }
}
