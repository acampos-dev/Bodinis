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

                _context.Database.EnsureCreated();

                SeedUsuarios();
                SeedCategoriasYProductos();

                Console.WriteLine(">>> SEED FINALIZADO EXITOSAMENTE <<<");
            }
            catch (Exception ex)
            {
                Console.WriteLine($">>> ERROR CRITICO EN SEED: {ex.Message}");
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

            var nombresCategorias = new[]
            {
                "Menus",
                "Pastas",
                "Milanesas",
                "Empanadas",
                "Tartas",
                "Frituras",
                "Bebidas",
                "Postres"
            };

            foreach (var nombreCat in nombresCategorias)
            {
                if (!_context.Categorias.Any(c => c.Nombre == nombreCat))
                {
                    _context.Categorias.Add(new Categoria(nombreCat, new List<Producto>()));
                }
            }
            _context.SaveChanges();

            var categorias = _context.Categorias.ToDictionary(c => c.Nombre, c => c);
            var nombresEnDb = _context.Productos
                .AsEnumerable()
                .Select(p => p.NombreProducto.Valor)
                .ToHashSet();

            Console.WriteLine(">>> VERIFICANDO PRODUCTOS...");

            // Menus
            AddProductoSiNoExiste("Salpicon de pollo con tomates", 390, 18, "Menus", categorias, nombresEnDb);
            AddProductoSiNoExiste("Muslo de pollo con boniato y papa al horno", 430, 20, "Menus", categorias, nombresEnDb);
            AddProductoSiNoExiste("Pastel de carne", 410, 16, "Menus", categorias, nombresEnDb);
            AddProductoSiNoExiste("Matambre a la leche con pure", 520, 12, "Menus", categorias, nombresEnDb);

            // Pastas
            AddProductoSiNoExiste("Sorrentinos con caruso", 460, 14, "Pastas", categorias, nombresEnDb);
            AddProductoSiNoExiste("Sorrentinos con bolognesa", 460, 14, "Pastas", categorias, nombresEnDb);
            AddProductoSiNoExiste("Tallarines caseros con bolognesa", 390, 18, "Pastas", categorias, nombresEnDb);

            // Milanesas
            AddProductoSiNoExiste("Milanesa al horno de carne", 380, 22, "Milanesas", categorias, nombresEnDb);
            AddProductoSiNoExiste("Milanesa al horno de pollo", 360, 22, "Milanesas", categorias, nombresEnDb);
            AddProductoSiNoExiste("Milanesa al pan de carne", 350, 24, "Milanesas", categorias, nombresEnDb);
            AddProductoSiNoExiste("Milanesa al pan de pollo", 330, 24, "Milanesas", categorias, nombresEnDb);

            // Empanadas
            AddProductoSiNoExiste("Empanada de carne", 85, 80, "Empanadas", categorias, nombresEnDb);
            AddProductoSiNoExiste("Empanada de jamon y queso", 85, 80, "Empanadas", categorias, nombresEnDb);
            AddProductoSiNoExiste("Empanada de carne y cheddar", 95, 70, "Empanadas", categorias, nombresEnDb);

            // Tartas
            AddProductoSiNoExiste("Tarta de jamon y queso", 340, 14, "Tartas", categorias, nombresEnDb);
            AddProductoSiNoExiste("Tarta pascualina", 340, 14, "Tartas", categorias, nombresEnDb);
            AddProductoSiNoExiste("Tarta de pollo", 350, 12, "Tartas", categorias, nombresEnDb);

            // Frituras
            AddProductoSiNoExiste("Bunuelos", 190, 22, "Frituras", categorias, nombresEnDb);

            // Bebidas
            AddProductoSiNoExiste("Coca Cola 1.5L", 220, 40, "Bebidas", categorias, nombresEnDb);
            AddProductoSiNoExiste("Coca Cola 600ml", 130, 50, "Bebidas", categorias, nombresEnDb);
            AddProductoSiNoExiste("Sprite 1.5L", 220, 30, "Bebidas", categorias, nombresEnDb);
            AddProductoSiNoExiste("Fanta 1.5L", 220, 30, "Bebidas", categorias, nombresEnDb);
            AddProductoSiNoExiste("Agua Salus 1.5L", 120, 45, "Bebidas", categorias, nombresEnDb);
            AddProductoSiNoExiste("Salus Frutte 1.5L", 170, 35, "Bebidas", categorias, nombresEnDb);

            // Postres (se venden por separado)
            AddProductoSiNoExiste("Ensalada de fruta", 170, 20, "Postres", categorias, nombresEnDb);
            AddProductoSiNoExiste("Arroz con leche", 160, 18, "Postres", categorias, nombresEnDb);
            AddProductoSiNoExiste("Crema de vainilla", 160, 18, "Postres", categorias, nombresEnDb);
            AddProductoSiNoExiste("Crema de vainilla y chocolate", 170, 18, "Postres", categorias, nombresEnDb);
            AddProductoSiNoExiste("Torta de manzana", 190, 15, "Postres", categorias, nombresEnDb);
            AddProductoSiNoExiste("Alfajor artesanal", 120, 30, "Postres", categorias, nombresEnDb);
            AddProductoSiNoExiste("Cookie artesanal", 110, 30, "Postres", categorias, nombresEnDb);

            _context.SaveChanges();
            Console.WriteLine($">>> PRODUCTOS PROCESADOS. TOTAL EN DB: {_context.Productos.Count()}");
        }

        private void AddProductoSiNoExiste(
            string nombre,
            int precio,
            int stock,
            string categoria,
            IReadOnlyDictionary<string, Categoria> categorias,
            ISet<string> nombresEnDb)
        {
            if (nombresEnDb.Contains(nombre))
            {
                return;
            }

            _context.Productos.Add(new Producto(
                new VoNombreProducto(nombre),
                new VoPrecio(precio),
                true,
                stock,
                categorias[categoria]));

            nombresEnDb.Add(nombre);
        }
    }
}
