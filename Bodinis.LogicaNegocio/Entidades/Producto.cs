

namespace Bodinis.LogicaNegocio.Entidades
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Precio { get; set; }
        public bool Disponible { get; set; }
        public int Stock { get; set; }

        public Producto() { } // Constructor vacio para EF

        public Producto(string nombre, int precio, bool disponible, int stock)
        {
            Nombre = nombre;
            Precio = precio;
            Disponible = disponible;
            Stock = stock;
        }

        
    }
}
