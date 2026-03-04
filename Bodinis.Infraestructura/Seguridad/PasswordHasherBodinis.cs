using Bodinis.LogicaNegocio.InterfacesLogicaNegocio;

namespace Bodinis.Infraestructura.Seguridad
{
    public class PasswordHasherBodinis : IPasswordHasher
    {
        public string Hash(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);
        public bool Verify(string passwordPlano, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordPlano) || string.IsNullOrWhiteSpace(passwordHash))
                return false;

            try
            {
                return BCrypt.Net.BCrypt.Verify(passwordPlano, passwordHash);
            }
            catch
            {
                // Si el hash almacenado está en un formato inválido,
                // se trata como credencial incorrecta en lugar de error 500.
                return false;
            }
        }
}
}

