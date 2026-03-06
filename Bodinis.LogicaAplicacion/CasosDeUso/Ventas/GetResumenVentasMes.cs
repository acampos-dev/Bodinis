using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
using Bodinis.LogicaNegocio.ModelosCasosUso;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Ventas
{
    public class GetResumenVentasMes : ICUGetResumenVentasMes
    {
        private readonly IRepositorioVenta _repoVenta;

        public GetResumenVentasMes(IRepositorioVenta repoVenta)
        {
            _repoVenta = repoVenta;
        }

        public ResumenVentasMes Execute(int anio, int mes)
        {
            var desde = new DateTime(anio, mes, 1, 0, 0, 0, DateTimeKind.Utc);
            var hasta = desde.AddMonths(1);
            var ventas = _repoVenta.GetByRango(desde, hasta).ToList();

            return new ResumenVentasMes(
                anio,
                mes,
                ventas.Count,
                ventas.Sum(v => v.TotalVenta));
        }
    }
}
