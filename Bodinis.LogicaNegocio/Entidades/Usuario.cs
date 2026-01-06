using Bodinis.LogicaNegocio.InterfacesLogicaNegocio;
using Bodinis.LogicaNegocio.Enums;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.Vo;


namespace Bodinis.LogicaNegocio.Entidades
{
    public class Usuario
    {
        public int Id { get; private set; }
        public string NombreCompleto { get;private set; }
        public VoEmail Email { get;private set; }
        public string UserName { get;private set; }
        public string PasswordHash { get;private set; }
        public bool Activo { get;private set; }
        public RolUsuario Rol{ get;private set; }

        public Usuario() { } // Constructor para EF

        public Usuario(string nombreCompleto,VoEmail email, string userName, string passwordHash, bool activo, RolUsuario rolUsuario)
        {
            
            NombreCompleto = nombreCompleto;
            Email = email;
            UserName = userName;
            PasswordHash = passwordHash;
            Activo = true;
            Rol = rolUsuario;
            Validar();
        }
         
        public void Desactivar()
        {
            Activo = false;
        }
        public void Activar()
        {
            Activo = true;
        }
        public void Validar() 
        {
            if(string.IsNullOrWhiteSpace(NombreCompleto))
            {
                throw new DatosInvalidosExcpetion("El nombre completo no puede estar vacío.");
            }
            if (string.IsNullOrWhiteSpace(UserName))
            {
                throw new DatosInvalidosExcpetion("El nombre de usuario no puede estar vacío.");
            }
            
        }

        public void ValidarLogin(bool passwordCorrecta)
        {
            
            if (!passwordCorrecta)
                throw new CredencialesInvalidasException("Contraseña incorrecta");

            if (!Activo)
                throw new UsuarioInactivoException("Usuario inactivo");
        }

    }
}
