namespace Bodinis.WebApp.Models
{
    public class LoginApiResponse
    {
        public int UsuarioId { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string RolUsuario { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
