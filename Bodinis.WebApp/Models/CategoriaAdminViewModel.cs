namespace Bodinis.WebApp.Models
{
    public class CategoriaAdminViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int CantidadProductos { get; set; }
        public bool PuedeEliminar => CantidadProductos == 0;
    }
}
