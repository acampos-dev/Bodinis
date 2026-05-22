using Bodinis.Infraestructura.AccesoDatos.Excepciones;
using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
using Microsoft.EntityFrameworkCore;

namespace Bodinis.Infraestructura.AccesoDatos.EF
{
    public class RepositorioMetodoPago : IRepositorioMetodoPago
    {
        private readonly BodinisContext _context;

        public RepositorioMetodoPago(BodinisContext context)
        {
            _context = context;
        }

        public void Add(MetodoPago metodoPago)
        {
            if (metodoPago == null)
            {
                throw new BadRequestException("El metodo de pago no puede ser nulo");
            }

            _context.MetodosPago.Add(metodoPago);
            _context.SaveChanges();
        }

        public IEnumerable<MetodoPago> GetAll()
        {
            return _context.MetodosPago
                .AsNoTracking()
                .OrderBy(m => m.Nombre)
                .ToList();
        }

        public MetodoPago? GetById(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("La id de metodo de pago es invalida");
            }

            return _context.MetodosPago.FirstOrDefault(m => m.Id == id);
        }

        public void Update(MetodoPago metodoPago)
        {
            if (metodoPago == null)
            {
                throw new BadRequestException("El metodo de pago no puede ser nulo");
            }

            _context.MetodosPago.Update(metodoPago);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var metodoPago = GetById(id)
                ?? throw new NotFoundException("No se encontro el metodo de pago");

            _context.MetodosPago.Remove(metodoPago);
            _context.SaveChanges();
        }
    }
}
