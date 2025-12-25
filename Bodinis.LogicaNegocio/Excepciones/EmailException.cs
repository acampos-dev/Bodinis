
namespace Bodinis.LogicaNegocio.Excepciones
{
    public class EmailException: LogicaNegocioException
    {
        public EmailException() { }
        public EmailException(string message) : base(message) { }
    }
}
