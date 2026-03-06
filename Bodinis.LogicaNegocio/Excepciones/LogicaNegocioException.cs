namespace Bodinis.LogicaNegocio.Excepciones
{
    public abstract class LogicaNegocioException : Exception
    {
        protected LogicaNegocioException()
            : base("Se produjo un error de validacion de negocio.")
        {
        }

        protected LogicaNegocioException(string message)
            : base(string.IsNullOrWhiteSpace(message)
                ? "Se produjo un error de validacion de negocio."
                : message)
        {
        }

        public Error Error()
        {
            return new Error(400, Message);
        }
    }
}
