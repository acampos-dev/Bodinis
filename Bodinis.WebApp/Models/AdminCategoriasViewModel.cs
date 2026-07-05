namespace Bodinis.WebApp.Models
{
    public class AdminCategoriasViewModel : AdminPageViewModel
    {
        public IReadOnlyList<CategoriaAdminViewModel> Categorias { get; set; } = Array.Empty<CategoriaAdminViewModel>();
        public string? SuccessMessage { get; set; }
        public int TotalCategorias => Categorias.Count;
        public int CategoriasEnUso => Categorias.Count(categoria => categoria.CantidadProductos > 0);
        public int CategoriasLibres => Categorias.Count(categoria => categoria.CantidadProductos == 0);
    }
}
