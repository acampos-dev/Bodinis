using Bodinis.LogicaNegocio.Enums;

namespace Bodinis.LogicaNegocio.Entidades
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public TipoPedido TipoPedido { get; set; }
        public EstadoPedido Estado { get; set; }

        public ICollection<DetallePedido> Detalles { get; set; }
        public int Total { get; set; }
        public Usuario Usuario { get; set; }

        public Pedido() { } // Constructor para EF

        public Pedido(DateTime fechaHora, TipoPedido tipoPedido, ICollection<DetallePedido> detalles , EstadoPedido estado, int total, Usuario usuario)
        {
            FechaHora = fechaHora;
            TipoPedido = tipoPedido;
            Estado = estado;
            Detalles = new List<DetallePedido>();
            Total = total;
            Usuario = usuario;
        }
    }
}
