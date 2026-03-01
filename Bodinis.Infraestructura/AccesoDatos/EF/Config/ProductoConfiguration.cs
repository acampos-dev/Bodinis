using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.Vo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bodinis.Infraestructura.AccesoDatos.EF.Config
{
    public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.ToTable("Productos");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.NombreProducto)
                .HasConversion(
                    v => v.Valor,
                    v => new VoNombreProducto(v))
                .HasColumnName("NombreProducto")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Precio)
                .HasConversion(
                    v => v.Valor,
                    v => new VoPrecio(v))
                .HasColumnName("Precio")
                .IsRequired();

            builder.Property(p => p.Disponible)
                .IsRequired();

            builder.Property(p => p.Stock)
                .IsRequired();

            builder.HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey("CategoriaId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}