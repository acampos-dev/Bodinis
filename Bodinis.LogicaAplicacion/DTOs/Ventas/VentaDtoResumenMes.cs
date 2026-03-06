namespace Bodinis.LogicaAplicacion.DTOs.Ventas
{
    public record VentaDtoResumenMes(
        int Anio,
        int Mes,
        int CantidadVentas,
        int TotalVendido);
}
