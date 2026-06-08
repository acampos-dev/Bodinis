using Bodinis.LogicaAplicacion.DTOs.Pedidos;
using Bodinis.LogicaNegocio.Enums;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Pedidos
{
    public class CambiarEstadoPedido : ICUCambiarEstadoPedido
    {
        private readonly IPedidoRepositorio _repoPedido;

        public CambiarEstadoPedido(IPedidoRepositorio repoPedido)
        {
            _repoPedido = repoPedido;
        }

        public void Execute(int pedidoId, EstadoPedido estado)
        {
            var pedido = _repoPedido.GetById(pedidoId)
                ?? throw new DatosInvalidosException("Pedido no encontrado");

            if (pedido.TipoPedido == TipoPedido.Mostrador)
            {
                throw new DatosInvalidosException("Los pedidos de mostrador se cobran automaticamente y no cambian de estado");
            }

            if (estado is not EstadoPedido.Pendiente and not EstadoPedido.Entregado)
            {
                throw new DatosInvalidosException("Los pedidos con delivery solo pueden estar pendientes o entregados");
            }

            if (estado == EstadoPedido.Entregado && pedido.Venta == null)
            {
                throw new DatosInvalidosException("Registra la venta del delivery para marcarlo como entregado");
            }

            if (estado == EstadoPedido.Pendiente && pedido.Venta != null)
            {
                throw new DatosInvalidosException("No se puede volver a pendiente un pedido ya cobrado");
            }

            pedido.Estado = estado;
            _repoPedido.Update(pedido);
        }

        public void Execute(int pedidoId, PedidoDtoCambiarEstado dto)
        {
            Execute(pedidoId, dto.Estado);
        }
    }
}
