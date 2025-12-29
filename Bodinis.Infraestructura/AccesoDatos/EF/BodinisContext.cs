using Bodinis.Infraestructura.AccesoDatos.EF.Config;
using Bodinis.LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Bodinis.Infraestructura.AccesoDatos.EF
{
    public class BodinisContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }


        public BodinisContext(DbContextOptions<BodinisContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
        }   
        
    }
}
