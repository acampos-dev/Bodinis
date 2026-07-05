namespace Bodinis.LogicaAplicacion.DTOs.Pedidos
{
    public record PedidoDetalleDtoListado(
        int ProductoId,
        string NombreProducto,
        int Cantidad,
        int PrecioUnitario,
        int Subtotal)
    {
    }
}
