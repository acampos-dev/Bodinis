using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bodinis.Infraestructura.AccesoDatos.Excepciones
{
    public class NotFoundException: InfraestructuraException
    {
        public NotFoundException() { }
        public NotFoundException(string message)
            : base(message) { }
        public override int StatusCode()
        {
            return 404;
        }
    }
}
