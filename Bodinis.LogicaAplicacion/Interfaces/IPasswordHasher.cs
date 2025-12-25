

namespace Bodinis.LogicaAplicacion.Interfaces
{
    public interface IPasswordHasher
    {
        string Hash(string passwordPlano);
    }
}
