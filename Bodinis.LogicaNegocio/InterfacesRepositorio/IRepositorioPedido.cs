using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.Enums;   

namespace Bodinis.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioPedido:
        IRepositorioAdd<Pedido>,
        IRepositorioGetById<Pedido>,
        IRepositorioUpdate<Pedido>,
        IRepositorioGetAll<Pedido>
    {
        IEnumerable<Pedido> GetByEstado(EstadoPedido estado);
        IEnumerable<Pedido> GetByFecha(DateOnly fecha);
        IEnumerable<Pedido> GetByRango(DateTime desdeInclusive, DateTime hastaExclusivo);
        int GetCantidadPedidosDelDia(DateOnly fecha);
         int GetTotalFacturadoDelDia(DateOnly fecha);
         int GetTicketPromedioDelDia(DateOnly fecha);
         int GetCantidadPedidosDeliveryDelDia(DateOnly fecha);
         int GetCantidadPedidosRetiroDelDia(DateOnly fecha);

        int GetTotalFacturadoDelMes(int anio, int mes);
        int GetTotalPedidosDelMes(int anio, int mes);
         int GetTicketPromedioDelMes(int anio, int mes);
         int GetCantidadPedidosDeliveryDelMes(int anio, int mes);
         int GetCantidadPedidosRetiroDelMes(int anio, int mes);
    }
}
