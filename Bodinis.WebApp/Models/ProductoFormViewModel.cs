using System.ComponentModel.DataAnnotations;

namespace Bodinis.WebApp.Models
{
    public class ProductoFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ingresa el nombre del producto.")]
        public string NombreProducto { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Ingresa un precio mayor a cero.")]
        public int Precio { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int Stock { get; set; }

        public bool Disponible { get; set; } = true;

        [Range(1, int.MaxValue, ErrorMessage = "Selecciona una categoria.")]
        public int CategoriaId { get; set; }
    }
}
