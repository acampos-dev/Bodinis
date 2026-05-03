using Bodinis.LogicaNegocio.Enums;

namespace Bodinis.LogicaAplicacion.DTOs.Pedidos
{
    public record PedidoDtoAlta(
        int UsuarioId,
        TipoPedido TipoPedido,
        string? NombreCliente,
        string? TelefonoCliente,
        string? DireccionCliente,
        List<PedidoDetalleDtoAlta> Detalles)
    {
    }
}
