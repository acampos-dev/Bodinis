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
                throw new BadRequestException("La venta no puede ser nula.");
            }

            _context.Ventas.Add(venta);
            _context.SaveChanges();
        }

        public IEnumerable<Venta> GetAll()
        {
            return _context.Ventas.ToList();
        }

        public Venta GetById(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Id inválido.");
            }

            var venta = _context.Ventas.FirstOrDefault(v => v.Id == id);
            if (venta == null)
            {
                throw new NotFoundException("No se encontró la venta.");
            }

            return venta;
        }

        public IEnumerable<Venta> GetByRango(DateTime desdeInclusive, DateTime hastaExclusivo)
        {
            return _context.Ventas
                .Where(v => v.FechaHora >= desdeInclusive && v.FechaHora < hastaExclusivo)
                .ToList();
        }

        public int GetTotalVentasDelDia(DateOnly fecha)
        {
            var desde = fecha.ToDateTime(TimeOnly.MinValue);
            var hasta = desde.AddDays(1);
            return _context.Ventas
                .Where(v => v.FechaHora >= desde && v.FechaHora < hasta)
                .Sum(v => v.TotalVenta);
        }

        public int GetTotalVentasDelMes(int anio, int mes)
        {
            return _context.Ventas
                .Where(v => v.FechaHora.Year == anio && v.FechaHora.Month == mes)
                .Sum(v => v.TotalVenta);
        }

        public int GetTotalVentasDelAnio(int anio)
        {
            return _context.Ventas
                .Where(v => v.FechaHora.Year == anio)
                .Sum(v => v.TotalVenta);
        }
    }
}
