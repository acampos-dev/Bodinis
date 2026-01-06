
namespace Bodinis.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface ICUGetById<T>
    {
        T Execute(int id);
    }
}
