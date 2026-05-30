namespace Bodinis.WebApp.Models
{
    public class ProductoAdminViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public int Precio { get; set; }
        public int Stock { get; set; }
        public bool Disponible { get; set; }
        public bool PuedeAgregarse => Disponible && Stock > 0;
    }
}
