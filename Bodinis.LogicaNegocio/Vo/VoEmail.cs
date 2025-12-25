

using Bodinis.LogicaNegocio.Excepciones;

namespace Bodinis.LogicaNegocio.Vo
{
    public record VoEmail
    {
        public string Email { get; init; }
        public VoEmail(string email)
        {
            Email = email;
            Validar();
        }
        private void Validar()
        {
            if (string.IsNullOrWhiteSpace(Email) || !Email.Contains("@"))
            {
                throw new EmailException("El email no es válido.");
            }
        }

        
    }
}
