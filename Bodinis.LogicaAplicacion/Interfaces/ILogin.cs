namespace Bodinis.LogicaAplicacion.Interfaces
{
    public interface ILogin<T>
    {
        string Execute(T obj);
    }
}
