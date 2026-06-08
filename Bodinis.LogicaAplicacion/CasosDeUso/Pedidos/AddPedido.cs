using Bodinis.LogicaAplicacion.DTOs.Pedidos;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.Enums;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
using MetodoPagoEntidad = Bodinis.LogicaNegocio.Entidades.MetodoPago;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Pedidos
{
    public class AddPedido : ICUAdd<PedidoDtoAlta>
    {
        private readonly IPedidoRepositorio _repoPedido;
        private readonly IRepositorioProducto _repoProducto;
        private readonly IRepositorioUsuario _repoUsuario;
        private readonly IRepositorioCaja _repoCaja;
        private readonly IRepositorioMetodoPago _repoMetodoPago;
        private readonly IRepositorioVenta _repoVenta;

        public AddPedido(
            IPedidoRepositorio repoPedido,
            IRepositorioProducto repoProducto,
            IRepositorioUsuario repoUsuario,
            IRepositorioCaja repoCaja,
            IRepositorioMetodoPago repoMetodoPago,
            IRepositorioVenta repoVenta)
        {
            _repoPedido = repoPedido;
            _repoProducto = repoProducto;
            _repoUsuario = repoUsuario;
            _repoCaja = repoCaja;
            _repoMetodoPago = repoMetodoPago;
            _repoVenta = repoVenta;
        }

        public void Execute(PedidoDtoAlta dto)
        {
            var caja = _repoCaja.GetCajaAbierta()
                ?? throw new CajaCerradaException("Abre una caja antes de registrar pedidos");

            var usuario = _repoUsuario.GetById(dto.UsuarioId)
                ?? throw new DatosInvalidosException("Usuario no encontrado");

            if (dto.TipoPedido == TipoPedido.Delivery && string.IsNullOrWhiteSpace(dto.DireccionCliente))
            {
                throw new DatosInvalidosException("La direccion es obligatoria para pedidos con delivery");
            }

            MetodoPagoEntidad? metodoPago = null;
            if (dto.TipoPedido == TipoPedido.Mostrador)
            {
                var metodoPagoId = dto.MetodoPagoId.GetValueOrDefault();
                if (metodoPagoId <= 0)
                {
                    throw new DatosInvalidosException("Selecciona un metodo de pago para pedidos de mostrador");
                }

                metodoPago = _repoMetodoPago.GetById(metodoPagoId)
                    ?? throw new DatosInvalidosException("Metodo de pago no encontrado");

                if (!metodoPago.Activo)
                {
                    throw new DatosInvalidosException("El metodo de pago no esta activo");
                }
            }

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

            if (pedido.TipoPedido == TipoPedido.Mostrador && metodoPago != null)
            {
                _repoVenta.Add(new Venta(DateTime.Now, pedido.Total, pedido, metodoPago, caja));
            }
        }
    }
}
