


namespace Bodinis.Infraestructura.AccesoDatos.Excepciones
{
    public class ConflictException: InfraestructuraException
    {
        public ConflictException() { }

        public ConflictException(string message)
            : base(message) { }

        public override int StatusCode()
        {
            return 409;
        }
    }
}
