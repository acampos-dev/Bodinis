using Bodinis.LogicaAplicacion.DTOs.Pedidos;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Pedidos
{
    public class GetPedidoTicketById : ICUGetById<PedidoDtoTicket>
    {
        private readonly IRepositorioPedido _repoPedido;

        public GetPedidoTicketById(IRepositorioPedido repoPedido)
        {
            _repoPedido = repoPedido;
        }

        public PedidoDtoTicket Execute(int id)
        {
            var pedido = _repoPedido.GetById(id);
            return PedidoReportesMapper.ToTicketDto(pedido, pedido.Usuario.NombreCompleto);
        }
    }
}
