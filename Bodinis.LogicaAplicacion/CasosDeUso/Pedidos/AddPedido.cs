using Bodinis.LogicaAplicacion.DTOs.Pedidos;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Pedidos
{
    public class AddPedido: ICUAdd<PedidoDtoAlta>
    {
        private readonly IPedidoRepositorio _repoPedido;
        private readonly IRepositorioProducto _repoProducto;
        private readonly IRepositorioUsuario _repoUsuario;

        public AddPedido(
            IPedidoRepositorio repoPedido,
            IRepositorioProducto repoProducto,
            IRepositorioUsuario repoUsuario)
        {
            _repoPedido = repoPedido;
            _repoProducto = repoProducto;
            _repoUsuario = repoUsuario;
        }

        public void Execute(PedidoDtoAlta dto)
        {
            var usuario = _repoUsuario.GetById(dto.UsuarioId)
                ?? throw new DatosInvalidosException("Usuario no encontrado");

            var detalles = new List<DetallePedido>();

            foreach (var detalleDto in dto.Detalles)
            {
                var producto = _repoProducto.GetById(detalleDto.ProductoId)
                    ?? throw new DatosInvalidosException($"Producto no encontrado: {detalleDto.ProductoId}");

                detalles.Add(new DetallePedido(
                    cantidad: detalleDto.Cantidad,
                    precioUnitario: producto.Precio.Valor,
                    producto: producto));
            }

            var pedido = PedidoMapper.ToEntity(dto, usuario, detalles);
            _repoPedido.Add(pedido);
        }
    }
    
}
