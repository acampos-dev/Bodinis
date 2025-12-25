using Bodinis.LogicaAplicacion.Interfaces;
using Bodinis.WepApi.Services;
using Bodinis.LogicaAplicacion.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace Libreria.WepApi.Services
{
    public class JwtGenerator : IJwtGenerator<LoginReponseDTO>
    {

        private readonly JwtSettings _settings;

        public JwtGenerator(JwtSettings settings)
        {
            _settings = settings;
        }

        public string GenerateToken(LoginReponseDTO usuario)
        {
            var key = Encoding.UTF8.GetBytes(_settings.Key);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol),

            // Podés agregar más claims como rol, nombre, etc.
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_settings.ExpirationMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
