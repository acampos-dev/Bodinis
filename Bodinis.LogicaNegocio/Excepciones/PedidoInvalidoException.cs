namespace Bodinis.LogicaNegocio.Excepciones
{
    public class PedidoInvalidoException : LogicaNegocioException
    {
        public PedidoInvalidoException()
            : base("El pedido es invalido.")
        {
        }

        public PedidoInvalidoException(string message)
            : base(message)
        {
        }
    }
}
