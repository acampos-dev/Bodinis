using Bodinis.LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bodinis.Infraestructura.AccesoDatos.EF.Config
{
    public class CajaConfiguration : IEntityTypeConfiguration<Caja>
    {
        public void Configure(EntityTypeBuilder<Caja> builder)
        {
            builder.ToTable("Cajas");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.FechaApertura).IsRequired();
            builder.Property(c => c.FechaCierre);
            builder.Property(c => c.MontoApertura).IsRequired();
            builder.Property(c => c.MontoCierre).IsRequired();

            builder.HasMany(c => c.Ventas)
                .WithOne()
                .HasForeignKey("CajaId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
