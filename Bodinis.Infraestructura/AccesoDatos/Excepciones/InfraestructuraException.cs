using System.Runtime.Serialization;
using Bodinis.LogicaNegocio.Excepciones;

namespace Bodinis.Infraestructura.AccesoDatos.Excepciones
{
    public abstract class InfraestructuraException: Exception
    {
        string _message;
        public InfraestructuraException() 
        {
        }
        public InfraestructuraException(string message) : base(message) 
        {
            _message = message;
        }
        protected InfraestructuraException(SerializationInfo info, StreamingContext context) : base(info, context) 
        {
        }

        public abstract int StatusCode();

        public Error Error()
        {
            return new Error(
            

               StatusCode(),
               _message
               );

        }   
    }
}
