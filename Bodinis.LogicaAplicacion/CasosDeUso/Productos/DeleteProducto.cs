using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
namespace Bodinis.LogicaAplicacion.CasosDeUso.Productos
{
    public class DeleteProducto: ICUDelete<Producto>
    {
        private readonly IRepositorioProducto _repositorioProducto;

        public DeleteProducto(IRepositorioProducto repositorioProducto)
        {
            _repositorioProducto = repositorioProducto;
        }

        public void Execute(int id)
        {
            var producto = _repositorioProducto.GetById(id);
            if(producto == null)
            {
                throw new Exception("Producto no encontrado");
            }
            _repositorioProducto.Delete(id);
        }

    }
}
