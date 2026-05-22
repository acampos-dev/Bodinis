using Bodinis.LogicaAplicacion.DTOs.Ventas;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Ventas
{
    public class GetAllVentas : ICUGetAll<VentaDtoListado>
    {
        private readonly IRepositorioVenta _repoVenta;

        public GetAllVentas(IRepositorioVenta repoVenta)
        {
            _repoVenta = repoVenta;
        }

        public IEnumerable<VentaDtoListado> Execute()
        {
            return VentaMapper.ToListDto(_repoVenta.GetAll());
        }
    }
}
