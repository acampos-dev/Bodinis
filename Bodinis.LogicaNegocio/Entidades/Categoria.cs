

namespace Bodinis.LogicaNegocio.Entidades
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public ICollection<Producto> Productos { get; set; }

        public Categoria() { }

        public Categoria(string nombre, ICollection<Producto> productos) {
            Nombre = nombre;
            Productos = new List<Producto>();
        }
    }
}
