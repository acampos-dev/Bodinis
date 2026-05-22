

namespace Bodinis.LogicaNegocio.Entidades
{
    public class MetodoPago
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();

        public MetodoPago() { } // Constructor para EF

        public MetodoPago(string nombre)
        {
            Nombre = nombre;
            Activo = true;
        }

        public void Modificar(string nombre, bool activo)
        {
            Nombre = nombre;
            Activo = activo;
        }
    }
}
