namespace Bodinis.WebApp.Models
{
    public class AdminPedidosViewModel : AdminPageViewModel
    {
        public IReadOnlyList<PedidoAdminViewModel> Pedidos { get; set; } = Array.Empty<PedidoAdminViewModel>();
        public IReadOnlyList<ProductoAdminViewModel> Productos { get; set; } = Array.Empty<ProductoAdminViewModel>();
        public IReadOnlyList<MetodoPagoOptionViewModel> MetodosPago { get; set; } = Array.Empty<MetodoPagoOptionViewModel>();
        public string? SuccessMessage { get; set; }
        public int PedidosActivos => Pedidos.Count(pedido => pedido.Estado is "Pendiente" or "Preparacion");
        public int TotalPendiente => Pedidos.Where(pedido => pedido.Estado is "Pendiente" or "Preparacion").Sum(pedido => pedido.Total);
    }
}
