

using Bodinis.LogicaNegocio.Excepciones;

namespace Bodinis.LogicaNegocio.Vo
{
    public class VoPrecio: LogicaNegocioException
    {
        public int Precio { get; init; }

        public VoPrecio(int precio)
        {
            Precio = precio;
            Validar();
        }

        private void Validar()
        {
            if (Precio < 0)
            {
                throw new PrecioException("El precio no puede ser negativo.");
            }
        }

    }
}
