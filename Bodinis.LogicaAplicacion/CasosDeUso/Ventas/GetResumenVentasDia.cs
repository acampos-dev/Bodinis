using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
using Bodinis.LogicaNegocio.ModelosCasosUso;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Ventas
{
    public class GetResumenVentasDia : ICUGetResumenVentasDia
    {
        private readonly IRepositorioVenta _repoVenta;

        public GetResumenVentasDia(IRepositorioVenta repoVenta)
        {
            _repoVenta = repoVenta;
        }

        public ResumenVentasDia Execute(DateOnly fecha)
        {
            var desde = fecha.ToDateTime(TimeOnly.MinValue);
            var hasta = desde.AddDays(1);
            var ventas = _repoVenta.GetByRango(desde, hasta).ToList();
            var cantidad = ventas.Count;
            var total = ventas.Sum(v => v.TotalVenta);
            var ticketPromedio = cantidad > 0 ? total / cantidad : 0;

            return new ResumenVentasDia(fecha, cantidad, total, ticketPromedio);
        }
    }
}
