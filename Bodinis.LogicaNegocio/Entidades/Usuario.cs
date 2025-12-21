

namespace Bodinis.LogicaNegocio.Entidades
{
    public class Usuario
    {
        public int Id { get; private set; }
        public string NombreCompleto { get; set; }
        public string UserName { get; set; }
        public string PaswordHash { get; set; }
        public bool Activo { get; set; }
        public RolUsuario Rol{ get; set; }

        public Usuario() { } // Constructor para EF

        public Usuario(string nombreCompleto, string userName, string paswordHash, bool activo, RolUsuario rolUsuario)
        {
            NombreCompleto = nombreCompleto;
            UserName = userName;
            PaswordHash = paswordHash;
            Activo = activo;
            Rol = rolUsuario;
        }
    }
}
