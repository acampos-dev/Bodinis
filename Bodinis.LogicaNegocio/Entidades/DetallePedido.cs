

namespace Bodinis.LogicaNegocio.Entidades
{
    public class DetallePedido
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public int PrecioUnitario { get; set; }
        public int Subtotal { get; set; }
        public int PedidoId { get; set; }
        public Pedido? Pedido { get; set; }
        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = null!;

        public DetallePedido() { } // Constructor para EF

        public DetallePedido(int cantidad, int precioUnitario, Producto producto)
        {
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
            Subtotal = cantidad * precioUnitario;
            Producto = producto;
            ProductoId = producto.Id;
        }
    }
}
