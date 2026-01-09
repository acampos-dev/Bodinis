
namespace Bodinis.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface ICUGetAll<T>
    {
        IEnumerable<T> Execute();
    }
}
