
namespace Bodinis.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioGetAll<T>
    {
        IEnumerable<T> GetAll();
    }
}
