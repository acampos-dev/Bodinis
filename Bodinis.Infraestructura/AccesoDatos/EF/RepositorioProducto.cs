using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
using Bodinis.LogicaNegocio.Vo;
using Bodinis.Infraestructura.AccesoDatos.Excepciones;

namespace Bodinis.Infraestructura.AccesoDatos.EF
{
    public class RepositorioProducto: IRepositorioProducto
    {
        private readonly BodinisContext _context;

        public RepositorioProducto(BodinisContext context)
        {
            _context = context;
        }

        
    }
}
