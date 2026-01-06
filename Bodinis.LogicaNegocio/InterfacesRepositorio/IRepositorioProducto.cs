using Bodinis.LogicaNegocio.Entidades;

namespace Bodinis.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioProducto:
        IRepositorioGetAll<Producto>,
        IRepositorioAdd<Producto>,
        IRepositorioGetById<Producto>,
        IRepositorioUpdate<Producto>,
        IRepositorioDelete<Producto>

    {

    }
}
