using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
using Bodinis.LogicaNegocio.ModelosCasosUso;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Pedidos
{
    public class GetResumenPedidos : ICUGetResumenPedidos
    {
        private readonly IRepositorioPedido _repoPedido;

        public GetResumenPedidos(IRepositorioPedido repoPedido)
        {
            _repoPedido = repoPedido;
        }

        public ResumenPedidosPeriodo Execute(DateOnly desde, DateOnly hasta)
        {
            var desdeDt = desde.ToDateTime(TimeOnly.MinValue);
            var hastaExclusivo = hasta.ToDateTime(TimeOnly.MinValue).AddDays(1);

            var pedidos = _repoPedido.GetByRango(desdeDt, hastaExclusivo).ToList();
            var cantidad = pedidos.Count;
            var total = pedidos.Sum(p => p.Total);
            var delivery = pedidos.Count(p => p.TipoPedido == Bodinis.LogicaNegocio.Enums.TipoPedido.Delivery);
            var retiro = pedidos.Count(p => p.TipoPedido == Bodinis.LogicaNegocio.Enums.TipoPedido.Mostrador);
            var ticketPromedio = cantidad > 0 ? total / cantidad : 0;

            return new ResumenPedidosPeriodo(
                desde,
                hasta,
                cantidad,
                total,
                ticketPromedio,
                delivery,
                retiro);
        }
    }
}
