using Bodinis.LogicaNegocio.Entidades;
namespace Bodinis.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioVenta
    {
        void Add (Venta venta);
        IEnumerable<Venta> GetAll();
        Venta? GetById(int id);
        IEnumerable<Venta> GetByFecha(DateTime desde, DateTime hasta);

    }
}
