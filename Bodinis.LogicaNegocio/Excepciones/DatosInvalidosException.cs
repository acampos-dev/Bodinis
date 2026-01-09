
namespace Bodinis.LogicaNegocio.Excepciones
{
    public class DatosInvalidosException: LogicaNegocioException
    {
        public DatosInvalidosException() { }
        public DatosInvalidosException(string message) : base(message)
        {
        }
    }
}
