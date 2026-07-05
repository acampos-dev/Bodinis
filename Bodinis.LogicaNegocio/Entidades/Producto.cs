

using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaNegocio;
using Bodinis.LogicaNegocio.Vo;

namespace Bodinis.LogicaNegocio.Entidades
{
    public class Producto: IValidable, IEquatable<Producto>
    {
        public int Id { get; private set; }
        public VoNombreProducto NombreProducto { get; private set; } = null!;
        public VoPrecio Precio { get; private set; } = null!;
        public bool Disponible { get; private set; }
        public int Stock { get; private set; }
        public Categoria Categoria { get; private set; } = null!;

        public Producto() { } // Constructor vacio para EF

        public Producto(
               VoNombreProducto nombreProducto, 
               VoPrecio precio, 
               bool disponible, 
               int stock,
               Categoria categoria)
        {
            NombreProducto = nombreProducto;
            Precio = precio;
            Disponible = disponible;
            Stock = stock;
            Categoria = categoria;
            Validar();
        }

        public void Validar()
        {
            if (Stock < 0)
            {
                throw new StockInvalidoException("El stock no puede ser negativo.");
            }
            if(NombreProducto == null)
            {
                throw new NombreProductoException();
            }
            if(Precio == null)
            {
                throw new PrecioException();
            }
            if(Categoria== null)
            {
                throw new CategoriaException("La categoria no puede ser nula.");
            }

        }

        public bool Equals(Producto? other)
        {
            if (other == null) return false;
            return Id.Equals(other.Id);
        }
        public void Modificar(
                VoNombreProducto nombre,
                VoPrecio precio,
                bool disponible,
                int stock,
                Categoria categoria)
        {
            NombreProducto = nombre;
            Precio = precio;
            Disponible = disponible;
            Stock = stock;
            Categoria = categoria;

            Validar();
        }


        public void Desactivar()
        {
            if (!Disponible)
                throw new DatosInvalidosException("El producto ya está desactivado");

            Disponible = false;
        }


    }
}
