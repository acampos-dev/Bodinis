using Bodinis.LogicaNegocio.Entidades;



namespace Bodinis.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioVenta:
        IRepositorioAdd<Venta>,
        IRepositorioGetById<Venta>,
        IRepositorioGetAll<Venta>
    {
        IEnumerable<Venta> GetByRango(DateTime desdeInclusive, DateTime hastaExclusivo);

        int GetTotalVentasDelDia(DateOnly fecha);
        int GetTotalVentasDelMes(int anio, int mes);
        int GetTotalVentasDelAnio(int anio);
    }
}
