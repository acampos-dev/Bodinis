using Bodinis.LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bodinis.Infraestructura.AccesoDatos.EF.Config
{
    public class GastoConfiguration : IEntityTypeConfiguration<Gasto>
    {
        public void Configure(EntityTypeBuilder<Gasto> builder)
        {
            builder.ToTable("Gastos");
            builder.HasKey(g => g.Id);

            builder.Property(g => g.FechaHora).IsRequired();
            builder.Property(g => g.Descripcion)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(g => g.Monto).IsRequired();
            builder.Property(g => g.Categoria).HasMaxLength(80);

            builder.HasOne(g => g.Caja)
                .WithMany(c => c.Gastos)
                .HasForeignKey(g => g.CajaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
