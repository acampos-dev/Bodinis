using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.Enums;
using Bodinis.LogicaNegocio.InterfacesLogicaNegocio;
using Bodinis.LogicaNegocio.Vo;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                Console.WriteLine(">>> EMPEZANDO SEED...");

                // Asegurar que la base de datos existe
                _context.Database.EnsureCreated();

                SeedUsuarios();
                SeedCategoriasYProductos();

                Console.WriteLine(">>> SEED FINALIZADO EXITOSAMENTE <<<");
            }
            catch (Exception ex)
            {
                Console.WriteLine($">>> ERROR CRÍTICO EN SEED: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($">>> DETALLE: {ex.InnerException.Message}");
                }
            }
        }

        private void SeedUsuarios()
        {
            Console.WriteLine(">>> VERIFICANDO USUARIOS...");

            if (_context.Usuarios.Any())
            {
                Console.WriteLine($">>> USUARIOS YA EXISTEN (Total: {_context.Usuarios.Count()})");
                return;
            }

            Console.WriteLine(">>> CREANDO USUARIOS POR DEFECTO...");

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
            Console.WriteLine(">>> USUARIOS CREADOS.");
        }

        private void SeedCategoriasYProductos()
        {
            Console.WriteLine(">>> VERIFICANDO CATEGORIAS...");

            // 1. Asegurar Categorías
            var nombresCategorias = new[] { "Pizzas", "Milanesas", "Bebidas" };
            foreach (var nombreCat in nombresCategorias)
            {
                if (!_context.Categorias.Any(c => c.Nombre == nombreCat))
                {
                    _context.Categorias.Add(new Categoria(nombreCat, new List<Producto>()));
                }
            }
            _context.SaveChanges();

            // 2. Obtener categorías en memoria para usarlas como referencia
            var categoriasExistentes = _context.Categorias.ToDictionary(c => c.Nombre, c => c);

            Console.WriteLine(">>> VERIFICANDO PRODUCTOS...");

            // 3. Obtener nombres de productos de forma segura (Evaluación en cliente para evitar error de traducción)
            var nombresEnDb = _context.Productos
                .AsEnumerable()
                .Select(p => p.NombreProducto.Valor)
                .ToList();

            // 4. Carga de productos

            // Pizza
            if (!nombresEnDb.Contains("Pizza Muzzarella"))
            {
                _context.Productos.Add(new Producto(
                    new VoNombreProducto("Pizza Muzzarella"),
                    new VoPrecio(450),
                    true,
                    20,
                    categoriasExistentes["Pizzas"]
                ));
            }

            // Milanesa
            if (!nombresEnDb.Contains("Milanesa Napolitana"))
            {
                _context.Productos.Add(new Producto(
                    new VoNombreProducto("Milanesa Napolitana"),
                    new VoPrecio(620),
                    true,
                    15,
                    categoriasExistentes["Milanesas"]
                ));
            }

            // Bebida
            if (!nombresEnDb.Contains("Coca Cola 1.5L"))
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
            Console.WriteLine($">>> PRODUCTOS PROCESADOS. TOTAL EN DB: {_context.Productos.Count()}");
        }
    }
}