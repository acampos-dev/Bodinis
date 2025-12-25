using Bodinis.LogicaNegocio.Excepciones;
namespace Bodinis.LogicaNegocio.Vo
{
    public record VoNombreProducto
    {
        public string NombreProducto { get; init; }

        public VoNombreProducto(string nombreProducto)
        {
            NombreProducto = nombreProducto;
            Validar();
        }

        private void Validar()
        {
            if (string.IsNullOrWhiteSpace(NombreProducto))
            {
                throw new NombreProductoException ("El nombre del producto no puede estar vacío.");
            }
            if (NombreProducto.Length > 100)
            {
                throw new NombreProductoException("El nombre del producto no puede exceder los 100 caracteres.");
            }
        }


    }
}
