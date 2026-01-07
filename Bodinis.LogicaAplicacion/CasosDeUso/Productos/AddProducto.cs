using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaAplicacion.DTOs.Productos;
namespace Bodinis.LogicaAplicacion.CasosDeUso.Productos
{
    public class AddProducto: ICUAdd<ProductoDtoAlta>
    {
        private IRepositorioProducto _repo;

        public AddProducto(IRepositorioProducto repo)
        {
            _repo = repo;
        }

        public void Execute(ProductoDtoAlta dto)
        {
            Producto producto = null; // Inicializo la variable producto

            if(dto)

        }
    }
}
