using Bodinis.LogicaAplicacion.DTOs.Categorias;
using Bodinis.LogicaNegocio.Entidades;

namespace Bodinis.LogicaAplicacion.Mappers
{
    public static class CategoriaMapper
    {
        public static Categoria ToEntity(CategoriaDtoAlta dto)
        {
            return new Categoria(dto.nombre, new List<Producto>());
        }

        public static CategoriaDtoListado ToDto(Categoria categoria)
        {
            return new CategoriaDtoListado(
                categoria.Id,
                categoria.Nombre,
                categoria.Productos?.Count ?? 0);
        }

        public static IEnumerable<CategoriaDtoListado> ToListDto(IEnumerable<Categoria> categorias)
        {
            return categorias.Select(ToDto);
        }
    }
}
