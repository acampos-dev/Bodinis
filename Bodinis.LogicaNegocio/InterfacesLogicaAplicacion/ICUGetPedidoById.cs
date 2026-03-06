using Bodinis.LogicaNegocio.Entidades;

namespace Bodinis.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface ICUGetPedidoById
    {
        Pedido Execute(int id);
    }
}
