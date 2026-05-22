using Bodinis.LogicaAplicacion.DTOs.MetodoPago;
using Bodinis.LogicaNegocio.Entidades;

namespace Bodinis.LogicaAplicacion.Mappers
{
    public static class MetodoPagoMapper
    {
        public static MetodoPago ToEntity(MetodoPagoDtoAlta dto)
        {
            return new MetodoPago(dto.Nombre);
        }

        public static MetodoPagoDtoListado ToDto(MetodoPago metodoPago)
        {
            return new MetodoPagoDtoListado(metodoPago.Id, metodoPago.Nombre, metodoPago.Activo);
        }

        public static IEnumerable<MetodoPagoDtoListado> ToListDto(IEnumerable<MetodoPago> metodosPago)
        {
            return metodosPago.Select(ToDto);
        }
    }
}
