using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Pedidos
{
    public class GetPedidoById : ICUGetPedidoById
    {
        private readonly IRepositorioPedido _repoPedido;

        public GetPedidoById(IRepositorioPedido repoPedido)
        {
            _repoPedido = repoPedido;
        }

        public Pedido Execute(int id)
        {
            return _repoPedido.GetById(id);
        }
    }
}
