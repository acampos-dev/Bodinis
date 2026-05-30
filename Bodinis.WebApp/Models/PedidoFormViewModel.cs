using System.ComponentModel.DataAnnotations;

namespace Bodinis.WebApp.Models
{
    public class PedidoFormViewModel
    {
        [Required]
        public int TipoPedido { get; set; } = 1;
        public string? NombreCliente { get; set; }
        public string? TelefonoCliente { get; set; }
        public string? DireccionCliente { get; set; }
        public int MetodoPagoId { get; set; }
        public List<int> ProductoIds { get; set; } = new();
        public List<int> Cantidades { get; set; } = new();
    }
}
