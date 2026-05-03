using Bodinis.LogicaNegocio.Entidades;
namespace Bodinis.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioVenta
    {
        void Add (Venta venta);
        IEnumerable<Venta> GetByFecha(DateTime desde, DateTime hasta);

    }
}
