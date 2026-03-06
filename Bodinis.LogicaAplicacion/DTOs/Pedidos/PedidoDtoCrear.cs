namespace Bodinis.LogicaAplicacion.DTOs.Pedidos
{
    public record PedidoDtoCrear(
        int UsuarioId,
        string TipoPedido,
        IReadOnlyCollection<PedidoDtoCrearItem> Items);

    public record PedidoDtoCrearItem(int ProductoId, int Cantidad);
}
