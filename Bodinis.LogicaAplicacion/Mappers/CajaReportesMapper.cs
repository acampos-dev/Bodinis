using Bodinis.LogicaAplicacion.DTOs.Cajas;
using Bodinis.LogicaNegocio.Entidades;

namespace Bodinis.LogicaAplicacion.Mappers
{
    public static class CajaReportesMapper
    {
        // Método para convertir a DTO de resumen de caja
        public static CajaDtoResumen ToResumenDto(
            int cajaId,
            DateTime fechaApertura,
            DateTime? fechaCierre,
            int montoApertura,
            int totalVentas,
            int montoCierre)
        {
            return new CajaDtoResumen(
                cajaId,
                fechaApertura,
                fechaCierre,
                montoApertura,
                totalVentas,
                montoCierre
                );
        }
        // Sobrecarga para convertir directamente desde la entidad Caja
        public static CajaDtoResumen ToResumenDto(Caja caja, DateTime? fechaCierre = null)
        {
            int totalVentas = caja.Ventas?.Sum(v => v.TotalVenta) ?? 0;

            return new CajaDtoResumen(
                caja.Id,
                caja.FechaApertura,
                fechaCierre,
                caja.MontoApertura,
                totalVentas,
                caja.MontoCierre);
        }
    }
}
