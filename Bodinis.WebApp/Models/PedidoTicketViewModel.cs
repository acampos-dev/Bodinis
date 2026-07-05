namespace Bodinis.WebApp.Models
{
    public class PedidoTicketViewModel
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public string TipoPedido { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string? NombreCliente { get; set; }
        public string? TelefonoCliente { get; set; }
        public string? DireccionCliente { get; set; }
        public string? MetodoPago { get; set; }
        public int Total { get; set; }
        public IReadOnlyList<PedidoDetalleAdminViewModel> Detalles { get; set; } = Array.Empty<PedidoDetalleAdminViewModel>();
        public bool EsDelivery => TipoPedido == "Delivery";
    }
}
