using Bodinis.LogicaNegocio.InterfacesLogicaNegocio;

namespace Bodinis.Infraestructura.Seguridad
{
    public class PasswordHasher : IPasswordHasher
    {
        public bool Verify(string passwordPlano, string passwordHash)
        => BCrypt.Net.BCrypt.Verify(passwordPlano, passwordHash);
    }
}
