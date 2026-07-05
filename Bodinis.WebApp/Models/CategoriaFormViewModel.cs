using System.ComponentModel.DataAnnotations;

namespace Bodinis.WebApp.Models
{
    public class CategoriaFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ingresa el nombre de la categoria.")]
        [StringLength(80, ErrorMessage = "El nombre no puede superar los 80 caracteres.")]
        public string Nombre { get; set; } = string.Empty;
    }
}
