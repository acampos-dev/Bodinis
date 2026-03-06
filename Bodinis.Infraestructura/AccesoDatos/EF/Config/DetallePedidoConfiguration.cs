using Bodinis.LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bodinis.Infraestructura.AccesoDatos.EF.Config
{
    public class DetallePedidoConfiguration : IEntityTypeConfiguration<DetallePedido>
    {
        public void Configure(EntityTypeBuilder<DetallePedido> builder)
        {
            builder.ToTable("DetallesPedido");
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Cantidad).IsRequired();
            builder.Property(d => d.PrecioUnitario).IsRequired();
            builder.Property(d => d.Subtotal).IsRequired();

            builder.HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey("ProductoId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
