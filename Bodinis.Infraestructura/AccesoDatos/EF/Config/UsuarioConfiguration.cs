using Bodinis.LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Bodinis.LogicaNegocio.Enums; // Asegúrate de tener el using del Enum

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

            // -----------------------------------------------------------
            // CAMBIO: De bool (1/0) a String ("SI"/"NO")
            // -----------------------------------------------------------
            builder.Property(u => u.Activo)
                   .IsRequired()
                   .HasConversion(
                        v => v ? "SI" : "NO",   // C# a DB
                        v => v == "SI"          // DB a C#
                   )
                   .HasMaxLength(2);

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

            // -----------------------------------------------------------
            // CAMBIO: Enum RolUsuario a String (ADMIN/EMPLEADO)
            // -----------------------------------------------------------
            builder.Property(u => u.Rol)
                   .IsRequired()
                   .HasConversion(
                        v => v.ToString().ToUpper(), // Guarda el nombre en mayúsculas
                        v => (RolUsuario)Enum.Parse(typeof(RolUsuario), v) // Recupera el Enum
                   )
                   .HasMaxLength(20);

            builder.HasIndex(u => u.UserName)
                   .IsUnique();
        }
    }
}