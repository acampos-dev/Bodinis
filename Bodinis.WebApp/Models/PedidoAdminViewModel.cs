namespace Bodinis.WebApp.Models
{
    public class PedidoAdminViewModel
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public string TipoPedido { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public int Total { get; set; }
    }
}
