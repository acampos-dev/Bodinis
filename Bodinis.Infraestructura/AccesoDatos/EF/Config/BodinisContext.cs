using Bodinis.LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Bodinis.Infraestructura.AccesoDatos.EF.Config;

namespace Bodinis.Infraestructura.AccesoDatos.EF.Config
{
    public class BodinisContext: DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }    
        public DbSet<Producto> Productos { get; set; }

    }
}
