

namespace Bodinis.LogicaNegocio.Entidades
{
    public class MetodoPago
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public MetodoPago() { } // Constructor para EF
        public MetodoPago(string nombre)
        {
            Nombre = nombre;
        }
    }
}
