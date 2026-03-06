using Bodinis.LogicaNegocio.Enums;
using Bodinis.LogicaNegocio.ModelosCasosUso;

namespace Bodinis.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface ICUCrearPedido
    {
        int Execute(int usuarioId, TipoPedido tipoPedido, IEnumerable<PedidoItemInput> items);
    }
}
