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

            SeedUsuarios();
            SeedCategoriasYProductos();

            Console.WriteLine(">>> SEED FINALIZADO <<<");
        }

        private void SeedUsuarios()
        {
            Console.WriteLine(">>> CONTANDO USUARIOS...");
            Console.WriteLine($">>> TOTAL: {_context.Usuarios.Count()}");

            if (_context.Usuarios.Any())
            {
                Console.WriteLine(">>> USUARIOS YA EXISTEN");
                return;
            }

            Console.WriteLine(">>> CREANDO USUARIOS POR DEFECTO");
            var admin = new Usuario(
                "Administrador Bodinis",
                new VoEmail("admin@bodinis.com"),
                "admin",
                _passwordHasher.Hash("Admin123"),
                true,
                RolUsuario.Admin
                );

            var empleado = new Usuario(
               "Empleado Bodinis",
               new VoEmail("empleado@bodinis.com"),
               "empleado",
               _passwordHasher.Hash("Empleado@123"),
               true,
               RolUsuario.Empleado
               );
            _context.Usuarios.AddRange(admin, empleado);
            _context.SaveChanges();
        }

        private void SeedCategoriasYProductos()
        {
            Console.WriteLine(">>> CONTANDO CATEGORIAS...");
            Console.WriteLine($">>> TOTAL: {_context.Categorias.Count()}");

            var categoriasExistentes = _context.Categorias
               .ToDictionary(c => c.Nombre, c => c);

            if (!categoriasExistentes.ContainsKey("Pizzas"))
            {
                categoriasExistentes["Pizzas"] = _context.Categorias.Add(new Categoria("Pizzas", new List<Producto>())).Entity;
            }

            if (!categoriasExistentes.ContainsKey("Milanesas"))
            {
                categoriasExistentes["Milanesas"] = _context.Categorias.Add(new Categoria("Milanesas", new List<Producto>())).Entity;
            }

            if (!categoriasExistentes.ContainsKey("Bebidas"))
            {
                categoriasExistentes["Bebidas"] = _context.Categorias.Add(new Categoria("Bebidas", new List<Producto>())).Entity;
            }

            _context.SaveChanges();

            Console.WriteLine(">>> CONTANDO PRODUCTOS...");
            Console.WriteLine($">>> TOTAL: {_context.Productos.Count()}");

            if (!_context.Productos.Any(p => p.NombreProducto.Valor == "Pizza Muzzarella"))
            {
                _context.Productos.Add(new Producto(
                    new VoNombreProducto("Pizza Muzzarella"),
                    new VoPrecio(450),
                    true,
                    20,
                    categoriasExistentes["Pizzas"]
                ));
            }

            if (!_context.Productos.Any(p => p.NombreProducto.Valor == "Milanesa Napolitana"))
            {
                _context.Productos.Add(new Producto(
                    new VoNombreProducto("Milanesa Napolitana"),
                    new VoPrecio(620),
                    true,
                    15,
                    categoriasExistentes["Milanesas"]
                ));
            }

            if (!_context.Productos.Any(p => p.NombreProducto.Valor == "Coca Cola 1.5L"))
            {
                _context.Productos.Add(new Producto(
                    new VoNombreProducto("Coca Cola 1.5L"),
                    new VoPrecio(210),
                    true,
                    40,
                    categoriasExistentes["Bebidas"]
                ));
            }

            _context.SaveChanges();
        }
    }
}
