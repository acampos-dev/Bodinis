using Bodinis.LogicaNegocio.Excepciones;
namespace Bodinis.LogicaNegocio.Vo
{
    public record VoNombreProducto
    {
        public string Valor { get; init; }

        public VoNombreProducto(string valor)
        {
            Valor = valor;
            Validar();
        }

        private void Validar()
        {
            if (string.IsNullOrWhiteSpace(Valor))
            {
                throw new NombreProductoException ("El nombre del producto no puede estar vacío.");
            }
            if (Valor.Length > 100)
            {
                throw new NombreProductoException("El nombre del producto no puede exceder los 100 caracteres.");
            }
        }


    }
}
