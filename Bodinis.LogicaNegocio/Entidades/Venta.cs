
namespace Bodinis.LogicaNegocio.Entidades
{
    public class Venta
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public int TotalVenta { get; set; }
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; } = null!;
        public int MetodoPagoId { get; set; }
        public MetodoPago MetodoPago { get; set; } = null!;
        public int CajaId { get; set; }
        public Caja Caja { get; set; } = null!;

        public Venta() { } // Constructor para EF

        public Venta(DateTime fechaHora, int totalVenta, Pedido pedido, MetodoPago metodoPago, Caja caja)
        {
            FechaHora = fechaHora;
            TotalVenta = totalVenta;
            Pedido = pedido;
            PedidoId = pedido.Id;
            MetodoPago = metodoPago;
            MetodoPagoId = metodoPago.Id;
            Caja = caja;
            CajaId = caja.Id;
        }

    }
}
