using Bodinis.LogicaNegocio.Entidades;

namespace Bodinis.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioCaja :
         IRepositorioAdd<Caja>,
         IRepositorioUpdate<Caja>,
         IRepositorioGetById<Caja>,
         IRepositorioGetAll<Caja>
    {
        Caja GetCajaAbierta();
        IEnumerable<Caja> GetCajasCerradas(DateTime desdeInclusive, DateTime hastaExclusivo);
        Caja GetCajaConVentas(int cajaId);
    }
}
