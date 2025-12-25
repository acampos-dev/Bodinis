

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

       
       
    }
}
