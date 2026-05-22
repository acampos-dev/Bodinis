using Bodinis.LogicaNegocio.Enums;

namespace Bodinis.LogicaNegocio.Entidades
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public TipoPedido TipoPedido { get; set; }
        public EstadoPedido Estado { get; set; }
        public string? NombreCliente { get; set; }
        public string? TelefonoCliente { get; set; }
        public string? DireccionCliente { get; set; }
        public ICollection<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();
        public int Total { get; set; }
        public Usuario Usuario { get; set; } = null!;
        public int UsuarioId { get; set; }
        public Venta? Venta { get; set; }

        public Pedido() { } // Constructor para EF

        public Pedido(
            DateTime fechaHora,
            TipoPedido tipoPedido,
            string? nombreCliente,
            string? telefonoCliente,
            string? direccionCliente,
            ICollection<DetallePedido> detalles,
            EstadoPedido estado,
            int total,
            Usuario usuario)
        {
            FechaHora = fechaHora;
            TipoPedido = tipoPedido;
            NombreCliente = nombreCliente;
            TelefonoCliente = telefonoCliente;
            DireccionCliente = direccionCliente;
            Estado = estado;
            Detalles = detalles ?? new List<DetallePedido>();
            Total = total;
            Usuario = usuario;
            UsuarioId = usuario.Id;
        }
    }
}
