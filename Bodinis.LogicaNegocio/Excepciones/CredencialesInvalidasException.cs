namespace Bodinis.LogicaNegocio.Excepciones
{
    public class CredencialesInvalidasException : LogicaNegocioException
    {
        public CredencialesInvalidasException()
            : base("Credenciales invalidas.")
        {
        }

        public CredencialesInvalidasException(string message)
            : base(message)
        {
        }
    }
}
