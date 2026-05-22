using Bodinis.LogicaAplicacion.DTOs.Gastos;
using Bodinis.LogicaNegocio.Entidades;

namespace Bodinis.LogicaAplicacion.Mappers
{
    public static class GastoMapper
    {
        public static Gasto ToEntity(GastoDtoAlta dto, Caja caja)
        {
            return new Gasto(DateTime.Now, dto.Descripcion, dto.Monto, dto.Categoria, caja);
        }

        public static GastoDtoListado ToDto(Gasto gasto)
        {
            return new GastoDtoListado(
                gasto.Id,
                gasto.FechaHora,
                gasto.Descripcion,
                gasto.Monto,
                gasto.Categoria,
                gasto.CajaId);
        }

        public static IEnumerable<GastoDtoListado> ToListDto(IEnumerable<Gasto> gastos)
        {
            return gastos.Select(ToDto);
        }
    }
}
