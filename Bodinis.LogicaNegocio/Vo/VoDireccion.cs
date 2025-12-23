using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bodinis.LogicaNegocio.Vo
{
    public record VoDireccion
    {
        public string Direccion { get; init; }

        public VoDireccion(string direccion)
        {
            if(string.IsNullOrWhiteSpace(direccion))
            {
                throw new ArgumentException("La dirección no puede estar vacía.");
            }
            Direccion = direccion;
        }

        public override bool Equals(object obj)
        {
            return obj is VoDireccion other &&
                   Direccion == other.Direccion;
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(Direccion);
        }
    }
}
