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

        public Caja? GetCajaAbierta()
        {
            return _context.Cajas
                .Include(c => c.Ventas)
                .Include(c => c.Gastos)
                .FirstOrDefault(c => c.EstaAbierta);
        }

        public Caja? GetById(int id)
        {
            return _context.Cajas
                .Include(c => c.Ventas)
                .Include(c => c.Gastos)
                .FirstOrDefault(c => c.Id == id);
        }

        public void AbrirCaja(Caja caja)
        {
            if (caja == null)
            {
                throw new BadRequestException("La caja no puede ser nula");
            }

            _context.Cajas.Add(caja);
            _context.SaveChanges();
        }

        public void CerrarCaja(Caja caja)
        {
            if (caja == null)
            {
                throw new BadRequestException("La caja no puede ser nula");
            }

            _context.Cajas.Update(caja);
            _context.SaveChanges();
        }
    }
}
