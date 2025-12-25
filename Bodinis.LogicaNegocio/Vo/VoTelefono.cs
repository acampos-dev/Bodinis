using Bodinis.LogicaNegocio.Excepciones;

namespace Bodinis.LogicaNegocio.Vo
{
    public record VoTelefono
    {
        public string Telefono { get; init; }
        public VoTelefono(string telefono)
        {
            Telefono = telefono;
            Validar();
        }
        private void Validar()
        {
            if (string.IsNullOrWhiteSpace(Telefono) || Telefono.Length < 7 || Telefono.Length > 15)
            {
                throw new TelefonoException("El teléfono no es válido.");
            }
        }
        
    }
}
