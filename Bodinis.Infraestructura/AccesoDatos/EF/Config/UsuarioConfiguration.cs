using Bodinis.LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bodinis.Infraestructura.AccesoDatos.EF.Config
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.NombreCompleto)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.UserName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.PasswordHash)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(u => u.Activo)
                   .IsRequired();

            // -------------------------
            // Value Object: Email
            // -------------------------
            builder.OwnsOne(u => u.Email, voEmail =>
            {
                voEmail.Property(e => e.Email)
                       .HasColumnName("Email")
                       .IsRequired()
                       .HasMaxLength(100);

                voEmail.HasIndex(e => e.Email)
                       .IsUnique();
            });

            // -------------------------
            // Enum RolUsuario
            // -------------------------
            builder.Property(u => u.Rol)
                   .IsRequired()
                   .HasConversion<int>();

            builder.HasIndex(u => u.UserName)
                   .IsUnique();
        }
    }
}
