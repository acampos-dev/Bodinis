using Bodinis.LogicaAplicacion.DTOs.MetodoPago;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.MetodoPago
{
    public class DeleteMetodoPago : ICUDelete<MetodoPagoDtoListado>
    {
        private readonly IRepositorioMetodoPago _repoMetodoPago;

        public DeleteMetodoPago(IRepositorioMetodoPago repoMetodoPago)
        {
            _repoMetodoPago = repoMetodoPago;
        }

        public void Execute(int id)
        {
            _repoMetodoPago.Delete(id);
        }
    }
}
