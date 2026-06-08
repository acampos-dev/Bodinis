namespace Bodinis.WebApp.Models
{
    public class AdminInicioViewModel : AdminPageViewModel
    {
        public IReadOnlyList<PedidoAdminViewModel> PedidosDelDia { get; set; } = Array.Empty<PedidoAdminViewModel>();
        public int ProductosDisponibles { get; set; }
        public string? SuccessMessage { get; set; }
        public int TotalPedidosHoy => PedidosDelDia.Count;
        public int DeliveryPendiente => PedidosDelDia.Count(pedido => pedido.TipoPedido == "Delivery" && pedido.Estado == "Pendiente");
        public int MostradorCobrado => PedidosDelDia.Count(pedido => pedido.TipoPedido == "Mostrador" && pedido.Estado == "Entregado");
        public int TotalCobradoHoy => PedidosDelDia.Where(pedido => pedido.Estado == "Entregado").Sum(pedido => pedido.Total);
    }
}
