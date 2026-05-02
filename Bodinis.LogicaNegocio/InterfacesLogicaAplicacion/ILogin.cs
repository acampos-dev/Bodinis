namespace Bodinis.LogicaAplicacion.Interfaces
{
    public interface ILogin<TRequest, TResponse> // Interfaz genérica para el caso de uso de login, con tipos de request y response parametrizados
    {
        TResponse Execute(TRequest obj); // Método para ejecutar la lógica de negocio del login
    }
}
