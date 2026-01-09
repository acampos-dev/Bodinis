using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaAplicacion.DTOs.Productos;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.Excepciones;
namespace Bodinis.LogicaAplicacion.CasosDeUso.Productos
{
    public class AddProducto: ICUAdd<ProductoDtoAlta>
    {
        private IRepositorioProducto _repoProducto;
        private IRepositorioCategoria _repoCategoria;

        public AddProducto(IRepositorioProducto repoProducto, 
                           IRepositorioCategoria repoCategoria)
        {
            _repoProducto = repoProducto;
            _repoCategoria = repoCategoria;
        }

        public void Execute(ProductoDtoAlta dto)
        {
            var categoria = _repoCategoria.GetById(dto.CategoriaId);

            if(categoria == null)
            {
                throw new DatosInvalidosException("Categoría no encontrada");
            }
            var producto = ProductoMapper.ToEntity(dto, categoria);

            _repoProducto.Add(producto);


        }
    }
}
