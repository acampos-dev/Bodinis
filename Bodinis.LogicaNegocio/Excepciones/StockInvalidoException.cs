
namespace Bodinis.LogicaNegocio.Excepciones
{
    public class StockInvalidoException: LogicaNegocioException
    {
        public StockInvalidoException() { }
        public StockInvalidoException(string message) : base(message) { }
    }
}
