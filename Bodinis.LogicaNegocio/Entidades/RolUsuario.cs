

namespace Bodinis.LogicaNegocio.Entidades
{
    public class RolUsuario
    {
        public int Id { get; set; }
        public string NombreRol { get; set; }

        public RolUsuario() { } // Constructor para EF

        public RolUsuario(string nombreRol)
        {
            NombreRol = nombreRol;
        }

    }
}
