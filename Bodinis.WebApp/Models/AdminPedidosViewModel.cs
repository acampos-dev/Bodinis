namespace Bodinis.WebApp.Models
{
    public class AdminPedidosViewModel : AdminPageViewModel
    {
        public IReadOnlyList<PedidoAdminViewModel> Pedidos { get; set; } = Array.Empty<PedidoAdminViewModel>();
        public IReadOnlyList<ProductoAdminViewModel> Productos { get; set; } = Array.Empty<ProductoAdminViewModel>();
        public IReadOnlyList<MetodoPagoOptionViewModel> MetodosPago { get; set; } = Array.Empty<MetodoPagoOptionViewModel>();
        public IReadOnlyList<CategoriaOptionViewModel> Categorias { get; set; } = Array.Empty<CategoriaOptionViewModel>();
        public string? SuccessMessage { get; set; }
        public int PedidosActivos => Pedidos.Count(pedido => pedido.TipoPedido == "Delivery" && pedido.Estado == "Pendiente");
        public int TotalPendiente => Pedidos.Where(pedido => pedido.TipoPedido == "Delivery" && pedido.Estado == "Pendiente").Sum(pedido => pedido.Total);
    }
}
