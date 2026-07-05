using Bodinis.Infraestructura.AccesoDatos.Excepciones;
using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
using Microsoft.EntityFrameworkCore;

namespace Bodinis.Infraestructura.AccesoDatos.EF
{
    public class RepositorioPedido : IPedidoRepositorio
    {
        private readonly BodinisContext _context;

        public RepositorioPedido(BodinisContext context)
        {
            _context = context;
        }

        public void Add(Pedido pedido)
        {
            if (pedido == null)
            {
                throw new BadRequestException("El pedido no puede ser nulo");
            }

            _context.Pedidos.Add(pedido);
            _context.SaveChanges();
        }

        public IEnumerable<Pedido> GetAll()
        {
            return _context.Pedidos
                .AsNoTracking()
                .Include(p => p.Usuario)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .Include(p => p.Venta)
                    .ThenInclude(v => v.MetodoPago)
                .OrderByDescending(p => p.FechaHora)
                .ToList();
        }

        public Pedido? GetById(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("La id de pedido es invalida");
            }

            return _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .Include(p => p.Venta)
                    .ThenInclude(v => v.MetodoPago)
                .FirstOrDefault(p => p.Id == id);
        }

        public void Update(Pedido pedido)
        {
            if (pedido == null)
            {
                throw new BadRequestException("El pedido no puede ser nulo");
            }

            _context.Pedidos.Update(pedido);
            _context.SaveChanges();
        }
    }
}
