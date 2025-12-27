using Bodinis.LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Bodinis.Infraestructura.AccesoDatos.EF.Config
{
    public class BodinisContext : DbContext
    {
        public BodinisContext(DbContextOptions<BodinisContext> options)
            : base(options)
        {
        }

        // =========================
        // DbSets
        // =========================
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuarios");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.NombreCompleto)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.UserName)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(u => u.PaswordHash)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(u => u.Activo)
                      .IsRequired();

                // -------- Value Object: Email --------
                entity.OwnsOne(u => u.Email, email =>
                {
                    email.Property(e => e.Email)
                         .HasColumnName("Email")
                         .IsRequired()
                         .HasMaxLength(100);
                });

                // -------- Enum Rol --------
                entity.Property(u => u.Rol)
                      .IsRequired()
                      .HasConversion<int>();

                // Índices
                entity.HasIndex("Email").IsUnique();
                entity.HasIndex(u => u.UserName).IsUnique();
            });
        }
    }
}
