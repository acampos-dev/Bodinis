using Bodinis.Infraestructura.AccesoDatos.Excepciones;
using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
using Microsoft.EntityFrameworkCore;

namespace Bodinis.Infraestructura.AccesoDatos.EF
{
    public class RepositorioGasto : IRepositorioGasto
    {
        private readonly BodinisContext _context;

        public RepositorioGasto(BodinisContext context)
        {
            _context = context;
        }

        public void Add(Gasto gasto)
        {
            if (gasto == null)
            {
                throw new BadRequestException("El gasto no puede ser nulo");
            }

            gasto.Validar();
            _context.Gastos.Add(gasto);
            _context.SaveChanges();
        }

        public IEnumerable<Gasto> GetByCaja(int cajaId)
        {
            return _context.Gastos
                .AsNoTracking()
                .Where(g => g.CajaId == cajaId)
                .OrderByDescending(g => g.FechaHora)
                .ToList();
        }

        public IEnumerable<Gasto> GetByFecha(DateTime desde, DateTime hasta)
        {
            return _context.Gastos
                .AsNoTracking()
                .Where(g => g.FechaHora >= desde && g.FechaHora <= hasta)
                .OrderByDescending(g => g.FechaHora)
                .ToList();
        }
    }
}
