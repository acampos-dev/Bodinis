using Bodinis.LogicaNegocio.Entidades;

namespace Bodinis.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioGasto
    {
        void Add(Gasto gasto);
        IEnumerable<Gasto> GetByCaja(int cajaId);
        IEnumerable<Gasto> GetByFecha(DateTime desde, DateTime hasta);
    }
}
