namespace Bodinis.LogicaAplicacion.Interfaces
{
    public interface IJwtGenerator<T>
    {
        string GenerateToken(T usuario);
    }
}
