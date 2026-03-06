using Bodinis.LogicaAplicacion.DTOs.Pedidos;

namespace Bodinis.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface ICUCrearPedido
    {
        PedidoDtoTicket Execute(PedidoDtoCrear dto);
    }
}
