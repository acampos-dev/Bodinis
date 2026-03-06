namespace Bodinis.LogicaNegocio.Excepciones
{
    public class UsuarioInactivoException : LogicaNegocioException
    {
        public UsuarioInactivoException()
            : base("Usuario inactivo.")
        {
        }

        public UsuarioInactivoException(string message)
            : base(message)
        {
        }
    }
}
