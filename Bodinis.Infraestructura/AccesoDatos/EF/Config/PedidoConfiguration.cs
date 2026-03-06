using Bodinis.LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bodinis.Infraestructura.AccesoDatos.EF.Config
{
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedidos");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.FechaHora).IsRequired();
            builder.Property(p => p.TipoPedido).HasConversion<int>().IsRequired();
            builder.Property(p => p.Estado).HasConversion<int>().IsRequired();
            builder.Property(p => p.Total).IsRequired();

            builder.HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey("UsuarioId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasMany(p => p.Detalles)
                .WithOne()
                .HasForeignKey("PedidoId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
