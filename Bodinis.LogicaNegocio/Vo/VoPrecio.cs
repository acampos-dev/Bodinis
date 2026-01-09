

using Bodinis.LogicaNegocio.Excepciones;

namespace Bodinis.LogicaNegocio.Vo
{
    public class VoPrecio: LogicaNegocioException
    {
        public int Valor { get; init; }

        public VoPrecio(int valor)
        {
            Valor = valor;
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
