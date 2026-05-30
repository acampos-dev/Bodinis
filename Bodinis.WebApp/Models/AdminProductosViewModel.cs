namespace Bodinis.WebApp.Models
{
    public class AdminProductosViewModel : AdminPageViewModel
    {
        public IReadOnlyList<ProductoAdminViewModel> Productos { get; set; } = Array.Empty<ProductoAdminViewModel>();
        public int TotalProductos => Productos.Count;
        public int ProductosDisponibles => Productos.Count(producto => producto.Disponible);
        public int StockTotal => Productos.Sum(producto => producto.Stock);
    }
}
