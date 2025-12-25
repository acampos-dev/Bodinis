namespace Bodinis.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioGetById<T>
    {
        T GetById(int id);
    }
}
