using Bodinis.LogicaNegocio.Enums;

namespace Bodinis.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface ICUCambiarEstadoPedido
    {
        void Execute(int pedidoId, EstadoPedido estado);
    }
}