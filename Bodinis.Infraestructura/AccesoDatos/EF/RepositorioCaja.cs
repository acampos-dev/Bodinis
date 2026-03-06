using Bodinis.Infraestructura.AccesoDatos.Excepciones;
using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
using Microsoft.EntityFrameworkCore;

namespace Bodinis.Infraestructura.AccesoDatos.EF
{
    public class RepositorioCaja : IRepositorioCaja
    {
        private readonly BodinisContext _context;

        public RepositorioCaja(BodinisContext context)
        {
            _context = context;
        }

        public void Add(Caja caja)
        {
            if (caja == null)
            {
                throw new BadRequestException("La caja no puede ser nula.");
            }

            _context.Cajas.Add(caja);
            _context.SaveChanges();
        }

        public IEnumerable<Caja> GetAll()
        {
            return _context.Cajas
                .Include(c => c.Ventas)
                .ToList();
        }

        public Caja GetById(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Id inválido.");
            }

            var caja = _context.Cajas
                .Include(c => c.Ventas)
                .FirstOrDefault(c => c.Id == id);

            if (caja == null)
            {
                throw new NotFoundException("No se encontró la caja.");
            }

            return caja;
        }

        public Caja GetCajaAbierta()
        {
            var caja = _context.Cajas
                .Include(c => c.Ventas)
                .FirstOrDefault(c => c.FechaCierre == null);

            if (caja == null)
            {
                throw new NotFoundException("No hay caja abierta.");
            }

            return caja;
        }

        public IEnumerable<Caja> GetCajasCerradas(DateTime desdeInclusive, DateTime hastaExclusivo)
        {
            return _context.Cajas
                .Include(c => c.Ventas)
                .Where(c => c.FechaCierre != null && c.FechaApertura >= desdeInclusive && c.FechaApertura < hastaExclusivo)
                .ToList();
        }

        public Caja GetCajaConVentas(int cajaId)
        {
            return GetById(cajaId);
        }

        public void Update(int id, Caja caja)
        {
            if (id <= 0 || caja == null)
            {
                throw new BadRequestException("Datos inválidos para actualizar caja.");
            }

            var cajaActual = _context.Cajas
                .Include(c => c.Ventas)
                .FirstOrDefault(c => c.Id == id);

            if (cajaActual == null)
            {
                throw new NotFoundException("No se encontró la caja.");
            }

            cajaActual.MontoCierre = caja.MontoCierre;
            cajaActual.FechaCierre = caja.FechaCierre;
            _context.SaveChanges();
        }
    }
}
