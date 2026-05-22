using Bodinis.LogicaAplicacion.DTOs.MetodoPago;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.MetodoPago
{
    public class GetAllMetodosPago : ICUGetAll<MetodoPagoDtoListado>
    {
        private readonly IRepositorioMetodoPago _repoMetodoPago;

        public GetAllMetodosPago(IRepositorioMetodoPago repoMetodoPago)
        {
            _repoMetodoPago = repoMetodoPago;
        }

        public IEnumerable<MetodoPagoDtoListado> Execute()
        {
            return MetodoPagoMapper.ToListDto(_repoMetodoPago.GetAll());
        }
    }
}
