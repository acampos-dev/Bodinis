using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.Enums;
using Bodinis.LogicaNegocio.InterfacesLogicaNegocio;
using Bodinis.LogicaNegocio.Vo;

namespace Bodinis.Infraestructura.AccesoDatos.EF.Seed
{
    public class SeedData
    {
        private readonly BodinisContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public SeedData(BodinisContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public void Run()
        {
            Console.WriteLine(">>> SEED DATA EJECUTADO <<<");
            Console.WriteLine(">>> CONTANDO USUARIOS...");
            Console.WriteLine($">>> TOTAL: {_context.Usuarios.Count()}");

            if (_context.Usuarios.Any())
            {
                Console.WriteLine(">>>YA EXISTEN USUARIOS");
                return; // Datos ya existen
            }

            Console.WriteLine(">>>CREANDO USUARIOS POR DEFECTO");
            var admin = new Usuario(
                "Administrador Bodinis",
                new VoEmail("admin@bodinis.com"),
                "admin",
                _passwordHasher.Hash("Admin123"),
                true,
                RolUsuario.Admin
                );

            var emopleado = new Usuario(
                "Empleado Bodinis",
                new VoEmail("empleado@bodinis.com"),
                "empleado",
                _passwordHasher.Hash("Empleado@123"),
                true,
                RolUsuario.Empleado
                );

            _context.Usuarios.AddRange(admin, emopleado);
            _context.SaveChanges();
            Console.WriteLine(">>> SEED FINALIZADO");

        }
    }
}
