

namespace Bodinis.LogicaNegocio.InterfacesLogicaNegocio
{
    public interface IPasswordHasher
    {
        string Hash(string passwordPlano);
        bool Verify(string passwordPlano, string passwordHash);
    }
}
