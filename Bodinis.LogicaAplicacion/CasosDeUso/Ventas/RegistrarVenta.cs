using Bodinis.LogicaAplicacion.DTOs.Ventas;
using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.Enums;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Ventas
{
    public class RegistrarVenta : ICUAdd<VentaDtoAlta>
    {
        private readonly IRepositorioVenta _repoVenta;
        private readonly IPedidoRepositorio _repoPedido;
        private readonly IRepositorioMetodoPago _repoMetodoPago;
        private readonly IRepositorioCaja _repoCaja;

        public RegistrarVenta(
            IRepositorioVenta repoVenta,
            IPedidoRepositorio repoPedido,
            IRepositorioMetodoPago repoMetodoPago,
            IRepositorioCaja repoCaja)
        {
            _repoVenta = repoVenta;
            _repoPedido = repoPedido;
            _repoMetodoPago = repoMetodoPago;
            _repoCaja = repoCaja;
        }

        public void Execute(VentaDtoAlta dto)
        {
            var caja = _repoCaja.GetCajaAbierta()
                ?? throw new CajaCerradaException("No se puede registrar una venta con la caja cerrada");

            var pedido = _repoPedido.GetById(dto.PedidoId)
                ?? throw new DatosInvalidosException("Pedido no encontrado");

            if (pedido.Venta != null)
            {
                throw new DatosInvalidosException("El pedido ya fue registrado como venta");
            }

            if (pedido.Estado == EstadoPedido.Cancelado)
            {
                throw new DatosInvalidosException("No se puede vender un pedido cancelado");
            }

            var metodoPago = _repoMetodoPago.GetById(dto.MetodoPagoId)
                ?? throw new DatosInvalidosException("Metodo de pago no encontrado");

            if (!metodoPago.Activo)
            {
                throw new DatosInvalidosException("El metodo de pago no esta activo");
            }

            pedido.Estado = EstadoPedido.Entregado;
            var venta = new Venta(DateTime.Now, pedido.Total, pedido, metodoPago, caja);

            _repoVenta.Add(venta);
        }
    }
}
