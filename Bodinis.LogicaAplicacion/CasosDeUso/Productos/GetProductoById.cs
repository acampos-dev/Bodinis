

using Bodinis.LogicaAplicacion.DTOs.Productos;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Productos
{
    public class GetProductoById : ICUGetById<ProductoDtoListado>
    {
        private readonly IRepositorioProducto _repo;

        public GetProductoById(IRepositorioProducto repo)
        {
            _repo = repo;
        }

        public ProductoDtoListado Execute(int id)
        {
            var producto = _repo.GetById(id);

            if (producto == null)
                throw new DatosInvalidosException("Producto no encontrado");

            return ProductoMapper.ToDto(producto);
        }
    }

}
