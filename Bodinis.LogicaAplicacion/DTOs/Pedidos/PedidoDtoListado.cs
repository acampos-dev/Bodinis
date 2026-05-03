using Bodinis.LogicaNegocio.Enums;

namespace Bodinis.LogicaAplicacion.DTOs.Pedidos
{
    public record PedidoDtoListado(int Id, 
                                   DateTime FechaHora, 
                                   TipoPedido TipoPedido, 
                                   EstadoPedido Estado, 
                                   int Total)
    {
    }
}
