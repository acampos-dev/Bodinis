
namespace Bodinis.LogicaNegocio.Excepciones
{
    public class UsuarioInactivoException: LogicaNegocioException
    {
        public UsuarioInactivoException() { }
        public UsuarioInactivoException(string message) : base(message)
        {
        }
    }
}
