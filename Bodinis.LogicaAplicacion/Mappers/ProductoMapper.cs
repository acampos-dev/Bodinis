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
                Id = producto.Id,
                NombreProducto = producto.NombreProducto.Valor,
                Precio = producto.Precio.Valor,
                Disponible = producto.Disponible,
                Stock = producto.Stock,
                Categoria = producto.Categoria.NombreCategoria
                );
        }
    }
}
