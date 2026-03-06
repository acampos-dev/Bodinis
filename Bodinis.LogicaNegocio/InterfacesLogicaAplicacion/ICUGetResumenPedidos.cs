using Bodinis.LogicaNegocio.ModelosCasosUso;

namespace Bodinis.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface ICUGetResumenPedidos
    {
        ResumenPedidosPeriodo Execute(DateOnly desde, DateOnly hasta);
    }
}
