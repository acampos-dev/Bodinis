
namespace Bodinis.LogicaNegocio.Excepciones
{
    public class DatosInvalidosExcpetion: LogicaNegocioException
    {
        public DatosInvalidosExcpetion() { }
        public DatosInvalidosExcpetion(string message) : base(message)
        {
        }
    }
}
