namespace Bodinis.LogicaAplicacion.Interfaces
{
    public interface ILogin<TRequest, TResponse>
    {
        TResponse Execute(TRequest obj);
    }
}
