using Bodinis.LogicaNegocio.InterfacesLogicaNegocio;

namespace Bodinis.Infraestructura.Seguridad
{
    public class PasswordHasherBodinis : IPasswordHasher
    {
        public string Hash(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);
        public bool Verify(string passwordPlano, string passwordHash)
        => BCrypt.Net.BCrypt.Verify(passwordPlano, passwordHash);
    }
}

