
namespace Bodinis.LogicaNegocio.Excepciones
{
    public class LogicaNegocioException: Exception
    {
        private string _message;
        public LogicaNegocioException()
        {
        }

        public LogicaNegocioException(string message) : base(message)
        {
            _message = message;
        }

        public Error Error()
        {
            return new Error(
                400,
                _message
                );
        }
    }
}
