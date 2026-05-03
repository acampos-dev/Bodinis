using Bodinis.LogicaAplicacion.DTOs.Pedidos;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Pedidos
{
    public class GetAllPedidos : ICUGetAll<PedidoDtoListado>
    {
        private readonly IPedidoRepositorio _repoPedido;

        public GetAllPedidos(IPedidoRepositorio repoPedido)
        {
            _repoPedido = repoPedido;
        }

        public IEnumerable<PedidoDtoListado> Execute()
        {
            var pedidos = _repoPedido.GetAll();
             
            return PedidoMapper.ToListDto(pedidos);
        }
    }
}