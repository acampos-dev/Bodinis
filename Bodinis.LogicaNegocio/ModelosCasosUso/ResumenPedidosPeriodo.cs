namespace Bodinis.LogicaNegocio.ModelosCasosUso
{
    public record ResumenPedidosPeriodo(
        DateOnly FechaDesde,
        DateOnly FechaHasta,
        int CantidadPedidos,
        int TotalFacturado,
        int TicketPromedio,
        int CantidadDelivery,
        int CantidadRetiro);
}
