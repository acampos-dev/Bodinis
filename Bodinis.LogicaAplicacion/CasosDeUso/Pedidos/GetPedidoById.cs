using Bodinis.LogicaAplicacion.DTOs.Pedidos;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Pedidos
{
    public class GetPedidoById : ICUGetById<PedidoDtoListado>
    {
        private readonly IPedidoRepositorio _repoPedido;

        public GetPedidoById(IPedidoRepositorio repoPedido)
        {
            _repoPedido = repoPedido;
        }

        public PedidoDtoListado Execute(int id)
        {
            var pedido = _repoPedido.GetById(id)
                ?? throw new DatosInvalidosException("Pedido no encontrado");

            return PedidoMapper.ToDto(pedido);
        }
    }
}