using Bodinis.LogicaAplicacion.DTOs.Ventas;

namespace Bodinis.LogicaAplicacion.Mappers
{
    public static class VentaReportesMapper
    {
        // / Convierte datos de ventas a un DTO de resumen por periodo
        public static VentaDtoResumenPeriodo ToResumenPeriodoDto(
            DateOnly fecha,
            int cantidadVentas,
            int totalVendido)
        {
            // Calcula el ticket promedio, evitando división por cero
            int ticketPromedio = cantidadVentas > 0 ? totalVendido / cantidadVentas : 0;

            return new VentaDtoResumenPeriodo(
                fecha,
                cantidadVentas,
                totalVendido,
                ticketPromedio);
        }
    }
}