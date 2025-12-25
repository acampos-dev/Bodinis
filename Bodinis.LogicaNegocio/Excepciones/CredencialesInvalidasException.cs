
namespace Bodinis.LogicaNegocio.Excepciones
{
    public class CredencialesInvalidasException: LogicaNegocioException
    {
        public CredencialesInvalidasException() { }
        public CredencialesInvalidasException(string message) : base(message)
        {
        }
    }
}
