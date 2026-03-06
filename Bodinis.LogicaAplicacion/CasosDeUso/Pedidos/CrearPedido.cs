using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.Enums;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
using Bodinis.LogicaNegocio.ModelosCasosUso;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Pedidos
{
    public class CrearPedido : ICUCrearPedido
    {
        private readonly IRepositorioPedido _repoPedido;
        private readonly IRepositorioProducto _repoProducto;
        private readonly IRepositorioUsuario _repoUsuario;
        private readonly IRepositorioCaja _repoCaja;

        public CrearPedido(
            IRepositorioPedido repoPedido,
            IRepositorioProducto repoProducto,
            IRepositorioUsuario repoUsuario,
            IRepositorioCaja repoCaja)
        {
            _repoPedido = repoPedido;
            _repoProducto = repoProducto;
            _repoUsuario = repoUsuario;
            _repoCaja = repoCaja;
        }

        public int Execute(int usuarioId, TipoPedido tipoPedido, IEnumerable<PedidoItemInput> items)
        {
            if (items == null || !items.Any())
            {
                throw new DatosInvalidosException("El pedido debe tener al menos un item.");
            }

            var cajaAbierta = _repoCaja.GetCajaAbierta();
            if (cajaAbierta.FechaCierre != null)
            {
                throw new CajaCerradaException("No se puede registrar un pedido con caja cerrada.");
            }

            var usuario = _repoUsuario.GetById(usuarioId);
            var detalles = new List<DetallePedido>();

            foreach (var item in items)
            {
                if (item.Cantidad <= 0)
                {
                    throw new DatosInvalidosException("La cantidad debe ser mayor a 0.");
                }

                var producto = _repoProducto.GetById(item.ProductoId);
                if (!producto.Disponible)
                {
                    throw new ProductoNoDisponibleException("El producto no está disponible.");
                }

                detalles.Add(new DetallePedido(
                    item.Cantidad,
                    producto.Precio.Valor,
                    producto));
            }

            var total = detalles.Sum(d => d.Subtotal);
            var pedido = new Pedido(
                DateTime.UtcNow,
                tipoPedido,
                detalles,
                EstadoPedido.Pendiente,
                total,
                usuario);

            _repoPedido.Add(pedido);

            cajaAbierta.Ventas.Add(new Venta(DateTime.UtcNow, total));
            _repoCaja.Update(cajaAbierta.Id, cajaAbierta);

            return pedido.Id;
        }
    }
}
