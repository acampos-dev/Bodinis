using Bodinis.LogicaAplicacion.DTOs.Productos;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Productos
{
    public class GetAllProductos : ICUGetAll<ProductoDtoListado>
    {
        private readonly IRepositorioProducto _repo;

        public GetAllProductos(IRepositorioProducto repo)
        {
            _repo = repo;
        }

        public IEnumerable<ProductoDtoListado> Execute()
        {
            var productos = _repo.GetAll();
            return ProductoMapper.ToListDto(productos);
        }
    }
}
