

using Bodinis.LogicaNegocio.Vo;

namespace Bodinis.LogicaNegocio.Entidades
{
    public class Producto
    {
        public int Id { get; set; }
        public VoNombreProducto NombreProducto { get; set; }
        public VoPrecio Precio { get; set; }
        public bool Disponible { get; set; }
        public int Stock { get; set; }

        public Producto() { } // Constructor vacio para EF

        public Producto(
               VoNombreProducto nombreProducto, 
               VoPrecio precio, 
               bool disponible, 
               int stock)
        {
            NombreProducto = nombreProducto;
            Precio = precio;
            Disponible = disponible;
            Stock = stock;
            Validar();
        }

        public void Validar()
        {

        }

        public bool Equals(Producto? other)
        {
            if (other == null) return false;
            return Id.Equals(other.Id);
        }


    }
}
