using Bodinis.LogicaNegocio.Vo;


namespace Bodinis.LogicaNegocio.Entidades
{
    public class Usuario
    {
        public int Id { get; private set; }
        public string NombreCompleto { get; set; }
        public VoEmail Email { get; set; }
        public string UserName { get; set; }
        public string PaswordHash { get; set; }
        public bool Activo { get; set; }
        public RolUsuario Rol{ get; set; }

        public Usuario() { } // Constructor para EF

        public Usuario(string nombreCompleto,VoEmail email, string userName, string paswordHash, bool activo, RolUsuario rolUsuario)
        {
            NombreCompleto = nombreCompleto;
            Email = email;
            UserName = userName;
            PaswordHash = paswordHash;
            Activo = activo;
            Rol = rolUsuario;
        }
    }
}
