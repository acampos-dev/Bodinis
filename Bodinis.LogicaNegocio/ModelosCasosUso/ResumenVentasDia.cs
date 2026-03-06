namespace Bodinis.LogicaNegocio.ModelosCasosUso
{
    public record ResumenVentasDia(
        DateOnly Fecha,
        int CantidadVentas,
        int TotalVendido,
        int TicketPromedio);
}
