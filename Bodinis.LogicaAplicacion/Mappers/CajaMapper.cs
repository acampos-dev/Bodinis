using Bodinis.LogicaAplicacion.DTOs.Caja;
using Bodinis.LogicaNegocio.Entidades;

namespace Bodinis.LogicaAplicacion.Mappers
{
    public static class CajaMapper
    {
        public static CajaDtoEstado ToEstadoDto(Caja caja)
        {
            return new CajaDtoEstado(
                caja.Id,
                caja.FechaApertura,
                caja.FechaCierre,
                caja.MontoApertura,
                caja.TotalVentas(),
                caja.TotalGastos(),
                caja.SaldoCalculado(),
                caja.MontoCierre,
                caja.EstaAbierta);
        }
    }
}
