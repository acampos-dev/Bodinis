using Bodinis.Infraestructura.AccesoDatos.EF.Config;
using Bodinis.LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Bodinis.Infraestructura.AccesoDatos.EF
{
    public class BodinisContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallesPedido { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Caja> Cajas { get; set; }

        public BodinisContext(DbContextOptions<BodinisContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriaConfiguration());
            modelBuilder.ApplyConfiguration(new ProductoConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoConfiguration());
            modelBuilder.ApplyConfiguration(new DetallePedidoConfiguration());
            modelBuilder.ApplyConfiguration(new VentaConfiguration());
            modelBuilder.ApplyConfiguration(new CajaConfiguration());
        }
    }
}
