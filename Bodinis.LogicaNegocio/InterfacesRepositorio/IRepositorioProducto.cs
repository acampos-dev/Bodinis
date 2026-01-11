using Bodinis.LogicaNegocio.Entidades;

namespace Bodinis.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioProducto :
    IRepositorioAdd<Producto>,
    IRepositorioUpdate<Producto>,
    IRepositorioGetById<Producto>,
    IRepositorioGetAll<Producto>
    {
        IEnumerable<Producto> GetActivos();
    }

}
