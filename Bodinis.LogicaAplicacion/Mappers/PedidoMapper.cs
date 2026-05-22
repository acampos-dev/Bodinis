using Bodinis.LogicaAplicacion.DTOs.Pedidos;
using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.Enums;

namespace Bodinis.LogicaAplicacion.Mappers
{
    public static class PedidoMapper
    {
        public static Pedido ToEntity(PedidoDtoAlta dto, Usuario usuario, IEnumerable<DetallePedido> detalles)
        {
            var detallesList = detalles.ToList();
            var total = detallesList.Sum(d => d.Subtotal);

            return new Pedido(
                fechaHora: DateTime.UtcNow,
                tipoPedido: dto.TipoPedido,
                nombreCliente: dto.NombreCliente,
                telefonoCliente: dto.TelefonoCliente,
                direccionCliente: dto.DireccionCliente,
                detalles: detallesList,
                estado: EstadoPedido.Pendiente,
                total: total,
                usuario: usuario);
        }

        public static PedidoDtoListado ToDto(Pedido pedido)
        {
            return new PedidoDtoListado(
                pedido.Id,
                pedido.FechaHora,
                pedido.TipoPedido,
                pedido.Estado,
                pedido.Total);
        }

        public static IEnumerable<PedidoDtoListado> ToListDto(IEnumerable<Pedido> pedidos)
        {
            return pedidos.Select(ToDto);
        }
    }
}
