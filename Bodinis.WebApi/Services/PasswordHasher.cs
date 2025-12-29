using Bodinis.LogicaAplicacion.Interfaces;
using Org.BouncyCastle.Crypto.Generators;

namespace Bodinis.WebApi.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
