namespace Bodinis.LogicaNegocio.Excepciones
{
    public class VentaInvalidaException : LogicaNegocioException
    {
        public VentaInvalidaException()
            : base("La venta es invalida.")
        {
        }

        public VentaInvalidaException(string message)
            : base(message)
        {
        }
    }
}
