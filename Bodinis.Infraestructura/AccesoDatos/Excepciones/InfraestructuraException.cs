using System.Runtime.Serialization;
using Bodinis.LogicaNegocio.Excepciones;

namespace Bodinis.Infraestructura.AccesoDatos.Excepciones
{
    public abstract class InfraestructuraException : Exception
    {
        protected InfraestructuraException()
            : base("Se produjo un error de infraestructura.")
        {
        }

        protected InfraestructuraException(string message)
            : base(string.IsNullOrWhiteSpace(message)
                ? "Se produjo un error de infraestructura."
                : message)
        {
        }

        protected InfraestructuraException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public abstract int StatusCode();

        public Error Error()
        {
            return new Error(StatusCode(), Message);
        }
    }
}
