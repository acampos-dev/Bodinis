using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.Vo;
using Bodinis.LogicaAplicacion.DTOs.Productos;

namespace Bodinis.LogicaAplicacion.Mappers
{
    public static class ProductoMapper
    {

        // DTO -> Entidad
        public static Producto ToEntity(
            ProductoDtoAlta dto,
            Categoria categoria)
        {
            return new Producto(
                new VoNombreProducto(dto.NombreProducto),
                new VoPrecio(dto.Precio),
                dto.Disponible,
                dto.Stock,
                categoria
                );
        }

        // Entidad -> DTO
        public static ProductoDtoListado ToDto(Producto producto)
        {
            return new ProductoDtoListado(
                producto.Id,
                producto.NombreProducto.Valor,
                producto.Precio.Valor,
                producto.Disponible,
                producto.Stock,
                producto.Categoria.Nombre
                );
        }

        // 🔹 Convierte una lista de entidades a una lista de DTOs
        public static IEnumerable<ProductoDtoListado> ToListDto(IEnumerable<Producto> producto)
        {
            List<ProductoDtoListado> listaDto = new List<ProductoDtoListado>();
            foreach (var item in producto)
            {
                listaDto.Add(ToDto(item));
            }
            return listaDto;
        }
}
