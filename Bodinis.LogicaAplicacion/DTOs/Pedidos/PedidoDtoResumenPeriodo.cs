

namespace Bodinis.LogicaAplicacion.DTOs.Pedidos
{
    public record PedidoDtoResumenPeriodo(
                                   DateOnly FechaDesde,
                                   DateOnly FechaHasta,
                                   int CantidadPedidos,
                                   int TotalFacturado,
                                   int TicketPromedio,
                                   int CantidadDelivery,
                                   int CantidadRetiro
                                  )
    {
    }
}
