using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.Enums;
using Bodinis.LogicaNegocio.Vo;

namespace Bodinis.Infraestructura.AccesoDatos.EF.Seed
{
    public class SeedData
    {
        private readonly BodinisContext _context;

        public SeedData(BodinisContext context)
        {
            _context = context;
        }

        public void Run()
        {
            if(_context.Usuarios.Any())
            {
                return; // Datos ya existen
            }

            var admin = new Usuario(
                "Administrador Bodinis",
                new VoEmail("admin@bodinis.com"),
                "admin",
                BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                true,
                RolUsuario.Admin
                );

            var emopleado = new Usuario(
                "Empleado Bodinis",
                new VoEmail("empleado@bodinis.com"),
                "empleado",
                BCrypt.Net.BCrypt.HashPassword("Empleado@123"),
                true,
                RolUsuario.Empleado
                );

            _context.Usuarios.AddRange(admin, emopleado);
            _context.SaveChanges();
        }
    }
}
