

namespace Bodinis.LogicaNegocio.InterfacesLogicaNegocio
{
    public interface IPasswordHasher
    {

        bool Verify(string passwordPlano, string passwordHash);
    }
}
