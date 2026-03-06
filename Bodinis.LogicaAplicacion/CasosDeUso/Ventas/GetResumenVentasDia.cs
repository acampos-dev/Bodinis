using Bodinis.LogicaAplicacion.DTOs.Ventas;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Ventas
{
    public class GetResumenVentasDia : ICUGetResumenVentasDia
    {
        private readonly IRepositorioVenta _repoVenta;

        public GetResumenVentasDia(IRepositorioVenta repoVenta)
        {
            _repoVenta = repoVenta;
        }

        public VentaDtoResumenPeriodo Execute(DateOnly fecha)
        {
            var desde = fecha.ToDateTime(TimeOnly.MinValue);
            var hasta = desde.AddDays(1);
            var ventas = _repoVenta.GetByRango(desde, hasta).ToList();

            return VentaReportesMapper.ToResumenPeriodoDto(
                fecha,
                ventas.Count,
                ventas.Sum(v => v.TotalVenta));
        }
    }
}
