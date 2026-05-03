using Bodinis.LogicaAplicacion.DTOs.Pedidos;
using Bodinis.LogicaNegocio.Enums;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Pedidos
{
    public class CambiarEstadoPedido: ICUCambiarEstadoPedido
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

            pedido.Estado = estado;
            _repoPedido.Update(pedido);
        }

        public void Execute(int pedidoId, PedidoDtoCambiarEstado dto)
        {
            Execute(pedidoId, dto.Estado);
        }
    }
}
