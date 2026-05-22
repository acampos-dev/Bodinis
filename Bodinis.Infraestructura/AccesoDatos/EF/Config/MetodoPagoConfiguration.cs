using Bodinis.LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bodinis.Infraestructura.AccesoDatos.EF.Config
{
    public class MetodoPagoConfiguration : IEntityTypeConfiguration<MetodoPago>
    {
        public void Configure(EntityTypeBuilder<MetodoPago> builder)
        {
            builder.ToTable("MetodosPago");
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Nombre)
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(m => m.Activo).IsRequired();

            builder.HasIndex(m => m.Nombre).IsUnique();
        }
    }
}
