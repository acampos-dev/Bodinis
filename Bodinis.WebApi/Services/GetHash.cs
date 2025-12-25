using Bodinis.LogicaAplicacion.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace Bodinis.WebApi.Services
{
    public class GetHash : IPasswordHasher
    {
        public string Hash(string passwordPlano)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(passwordPlano);
            var hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
