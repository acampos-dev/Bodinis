using Bodinis.LogicaAplicacion.DTOs.Pedidos;

namespace Bodinis.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface ICUGetResumenPedidos
    {
        PedidoDtoResumenPeriodo Execute(DateOnly desde, DateOnly hasta);
    }
}
