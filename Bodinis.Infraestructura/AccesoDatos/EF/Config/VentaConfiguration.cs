using Bodinis.LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bodinis.Infraestructura.AccesoDatos.EF.Config
{
    public class VentaConfiguration : IEntityTypeConfiguration<Venta>
    {
        public void Configure(EntityTypeBuilder<Venta> builder)
        {
            builder.ToTable("Ventas");
            builder.HasKey(v => v.Id);

            builder.Property(v => v.FechaHora).IsRequired();
            builder.Property(v => v.TotalVenta).IsRequired();

            builder.HasOne(v => v.Pedido)
                .WithOne(p => p.Venta)
                .HasForeignKey<Venta>(v => v.PedidoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(v => v.MetodoPago)
                .WithMany(m => m.Ventas)
                .HasForeignKey(v => v.MetodoPagoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(v => v.Caja)
                .WithMany(c => c.Ventas)
                .HasForeignKey(v => v.CajaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
