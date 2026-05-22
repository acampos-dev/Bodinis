using Bodinis.LogicaAplicacion.DTOs.Ventas;
using Bodinis.LogicaNegocio.Entidades;

namespace Bodinis.LogicaAplicacion.Mappers
{
    public static class VentaMapper
    {
        public static VentaDtoListado ToDto(Venta venta)
        {
            return new VentaDtoListado(
                venta.Id,
                venta.FechaHora,
                venta.TotalVenta,
                venta.PedidoId,
                venta.MetodoPago?.Nombre ?? string.Empty,
                venta.CajaId);
        }

        public static IEnumerable<VentaDtoListado> ToListDto(IEnumerable<Venta> ventas)
        {
            return ventas.Select(ToDto);
        }
    }
}
