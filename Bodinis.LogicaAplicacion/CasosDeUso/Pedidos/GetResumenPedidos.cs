using Bodinis.LogicaAplicacion.DTOs.Pedidos;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Pedidos
{
    public class GetResumenPedidos : ICUGetResumenPedidos
    {
        private readonly IRepositorioPedido _repoPedido;

        public GetResumenPedidos(IRepositorioPedido repoPedido)
        {
            _repoPedido = repoPedido;
        }

        public PedidoDtoResumenPeriodo Execute(DateOnly desde, DateOnly hasta)
        {
            var desdeDt = desde.ToDateTime(TimeOnly.MinValue);
            var hastaExclusivo = hasta.ToDateTime(TimeOnly.MinValue).AddDays(1);

            var pedidos = _repoPedido.GetByRango(desdeDt, hastaExclusivo).ToList();
            var cantidad = pedidos.Count;
            var total = pedidos.Sum(p => p.Total);
            var delivery = pedidos.Count(p => p.TipoPedido == Bodinis.LogicaNegocio.Enums.TipoPedido.Delivery);
            var retiro = pedidos.Count(p => p.TipoPedido == Bodinis.LogicaNegocio.Enums.TipoPedido.Mostrador);

            return PedidoReportesMapper.ToResumenPeriodoDto(desde, hasta, cantidad, total, delivery, retiro);
        }
    }
}
