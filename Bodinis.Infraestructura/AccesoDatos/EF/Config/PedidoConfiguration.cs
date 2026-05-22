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
            builder.Property(p => p.TipoPedido).HasConversion<string>().HasMaxLength(30).IsRequired();
            builder.Property(p => p.Estado).HasConversion<string>().HasMaxLength(30).IsRequired();
            builder.Property(p => p.NombreCliente).HasMaxLength(100);
            builder.Property(p => p.TelefonoCliente).HasMaxLength(30);
            builder.Property(p => p.DireccionCliente).HasMaxLength(200);
            builder.Property(p => p.Total).IsRequired();

            builder.HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Detalles)
                .WithOne(d => d.Pedido)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
