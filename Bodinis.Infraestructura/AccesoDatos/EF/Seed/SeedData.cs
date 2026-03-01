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
            Console.WriteLine(">>> SEED DATA EJECUTADO <<<");
            Console.WriteLine($">>> DB SERVER: {_context.Database.GetDbConnection().DataSource}");
            Console.WriteLine($">>> DB NAME: {_context.Database.GetDbConnection().Database}");

            SeedUsuarios();
            SeedCategoriasYProductos();

            Console.WriteLine($">>> CONTEO FINAL USUARIOS: {_context.Usuarios.Count()}");
            Console.WriteLine($">>> CONTEO FINAL CATEGORIAS: {_context.Categorias.Count()}");
            Console.WriteLine($">>> CONTEO FINAL PRODUCTOS: {_context.Productos.Count()}");
            Console.WriteLine(">>> SEED FINALIZADO <<<");
        }

        private void SeedUsuarios()
        {
            Console.WriteLine(">>> CONTANDO USUARIOS...");
            Console.WriteLine($">>> TOTAL: {_context.Usuarios.Count()}");

            if (!ExisteUsuarioPorEmail("admin@bodinis.com"))
            {
                Console.WriteLine(">>> CREANDO USUARIO ADMIN");
                _context.Usuarios.Add(new Usuario(
                    "Administrador Bodinis",
                    new VoEmail("admin@bodinis.com"),
                    "admin",
                    _passwordHasher.Hash("Admin123"),
                    true,
                    RolUsuario.Admin
                ));
            }

            if (!ExisteUsuarioPorEmail("empleado@bodinis.com"))
            {
                Console.WriteLine(">>> CREANDO USUARIO EMPLEADO");
                _context.Usuarios.Add(new Usuario(
                    "Empleado Bodinis",
                    new VoEmail("empleado@bodinis.com"),
                    "empleado",
                    _passwordHasher.Hash("Empleado@123"),
                    true,
                    RolUsuario.Empleado
                ));
            }

            _context.SaveChanges();
            Console.WriteLine($">>> USUARIOS LUEGO DE SEED: {_context.Usuarios.Count()}");
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
            Console.WriteLine($">>> CATEGORIAS LUEGO DE SEED: {_context.Categorias.Count()}");

            Console.WriteLine(">>> CONTANDO PRODUCTOS...");
            Console.WriteLine($">>> TOTAL: {_context.Productos.Count()}");

            if (!ExisteProductoPorNombre("Pizza Muzzarella"))
            {
                _context.Productos.Add(new Producto(
                    new VoNombreProducto("Pizza Muzzarella"),
                    new VoPrecio(450),
                    true,
                    20,
                    categoriasExistentes["Pizzas"]
                ));
            }

            if (!ExisteProductoPorNombre("Milanesa Napolitana"))
            {
                _context.Productos.Add(new Producto(
                    new VoNombreProducto("Milanesa Napolitana"),
                    new VoPrecio(620),
                    true,
                    15,
                    categoriasExistentes["Milanesas"]
                ));
            }

            if (!ExisteProductoPorNombre("Coca Cola 1.5L"))
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
            Console.WriteLine($">>> PRODUCTOS LUEGO DE SEED: {_context.Productos.Count()}");
        }

        private bool ExisteUsuarioPorEmail(string email)
        {
            return _context.Usuarios.Any(u =>
                EF.Property<string>(u, "Email") == email);
        }

        private bool ExisteProductoPorNombre(string nombreProducto)
        {
            return _context.Productos.Any(p =>
                EF.Property<string>(p, "NombreProducto") == nombreProducto);
        }
    }
}
