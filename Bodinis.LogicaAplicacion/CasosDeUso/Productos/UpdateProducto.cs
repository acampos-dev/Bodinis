using Bodinis.LogicaAplicacion.DTOs.Productos;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
using Bodinis.LogicaNegocio.Vo;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Productos
{
    public class UpdateProducto : ICUUpdate<ProductoDtoModificar>
    {
        private readonly IRepositorioProducto _repoProducto;
        private readonly IRepositorioCategoria _repoCategoria;

        public UpdateProducto(
            IRepositorioProducto repoProducto,
            IRepositorioCategoria repoCategoria)
        {
            _repoProducto = repoProducto;
            _repoCategoria = repoCategoria;
        }

        public void Execute(int id, ProductoDtoModificar dto)
        {
            var producto = _repoProducto.GetById(id);
            if (producto == null)
                throw new DatosInvalidosException("Producto no existe");

            var categoria = _repoCategoria.GetById(dto.CategoriaId);
            if (categoria == null)
                throw new DatosInvalidosException("Categoría inválida");

            producto.Modificar(
                new VoNombreProducto(dto.NombreProducto),
                new VoPrecio(dto.Precio),
                dto.Disponible,
                dto.Stock,
                categoria
            );

            _repoProducto.Update(id, producto);
        }
    }
}
