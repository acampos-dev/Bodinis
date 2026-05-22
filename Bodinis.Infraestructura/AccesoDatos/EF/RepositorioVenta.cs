using Bodinis.Infraestructura.AccesoDatos.Excepciones;
using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
using Microsoft.EntityFrameworkCore;

namespace Bodinis.Infraestructura.AccesoDatos.EF
{
    public class RepositorioVenta : IRepositorioVenta
    {
        private readonly BodinisContext _context;

        public RepositorioVenta(BodinisContext context)
        {
            _context = context;
        }

        public void Add(Venta venta)
        {
            if (venta == null)
            {
                throw new BadRequestException("La venta no puede ser nula");
            }

            _context.Ventas.Add(venta);
            _context.SaveChanges();
        }

        public IEnumerable<Venta> GetAll()
        {
            return _context.Ventas
                .AsNoTracking()
                .Include(v => v.Pedido)
                .Include(v => v.MetodoPago)
                .Include(v => v.Caja)
                .OrderByDescending(v => v.FechaHora)
                .ToList();
        }

        public Venta? GetById(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("La id de venta es invalida");
            }

            return _context.Ventas
                .Include(v => v.Pedido)
                .Include(v => v.MetodoPago)
                .Include(v => v.Caja)
                .FirstOrDefault(v => v.Id == id);
        }

        public IEnumerable<Venta> GetByFecha(DateTime desde, DateTime hasta)
        {
            return _context.Ventas
                .AsNoTracking()
                .Include(v => v.MetodoPago)
                .Where(v => v.FechaHora >= desde && v.FechaHora <= hasta)
                .OrderByDescending(v => v.FechaHora)
                .ToList();
        }
    }
}
