using Bodinis.Infraestructura.AccesoDatos.Excepciones;
using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.Enums;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
using Microsoft.EntityFrameworkCore;

namespace Bodinis.Infraestructura.AccesoDatos.EF
{
    public class RepositorioPedido : IRepositorioPedido
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
                throw new BadRequestException("El pedido no puede ser nulo.");
            }

            _context.Pedidos.Add(pedido);
            _context.SaveChanges();
        }

        public IEnumerable<Pedido> GetAll()
        {
            return _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .ToList();
        }

        public Pedido GetById(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Id inválido.");
            }

            var pedido = _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefault(p => p.Id == id);

            if (pedido == null)
            {
                throw new NotFoundException("No se encontró el pedido.");
            }

            return pedido;
        }

        public IEnumerable<Pedido> GetByEstado(EstadoPedido estado)
        {
            return _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .Where(p => p.Estado == estado)
                .ToList();
        }

        public IEnumerable<Pedido> GetByFecha(DateOnly fecha)
        {
            var desde = fecha.ToDateTime(TimeOnly.MinValue);
            var hasta = desde.AddDays(1);
            return GetByRango(desde, hasta);
        }

        public IEnumerable<Pedido> GetByRango(DateTime desdeInclusive, DateTime hastaExclusivo)
        {
            return _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .Where(p => p.FechaHora >= desdeInclusive && p.FechaHora < hastaExclusivo)
                .ToList();
        }

        public void Update(int id, Pedido obj)
        {
            if (id <= 0 || obj == null)
            {
                throw new BadRequestException("Datos inválidos para actualizar pedido.");
            }

            var pedidoActual = _context.Pedidos.FirstOrDefault(p => p.Id == id);
            if (pedidoActual == null)
            {
                throw new NotFoundException("No se encontró el pedido.");
            }

            pedidoActual.Estado = obj.Estado;
            _context.SaveChanges();
        }

        public int GetCantidadPedidosDelDia(DateOnly fecha)
        {
            var desde = fecha.ToDateTime(TimeOnly.MinValue);
            var hasta = desde.AddDays(1);
            return _context.Pedidos.Count(p => p.FechaHora >= desde && p.FechaHora < hasta);
        }

        public int GetTotalFacturadoDelDia(DateOnly fecha)
        {
            var desde = fecha.ToDateTime(TimeOnly.MinValue);
            var hasta = desde.AddDays(1);
            return _context.Pedidos
                .Where(p => p.FechaHora >= desde && p.FechaHora < hasta)
                .Sum(p => p.Total);
        }

        public int GetTicketPromedioDelDia(DateOnly fecha)
        {
            var cantidad = GetCantidadPedidosDelDia(fecha);
            if (cantidad == 0)
            {
                return 0;
            }

            return GetTotalFacturadoDelDia(fecha) / cantidad;
        }

        public int GetCantidadPedidosDeliveryDelDia(DateOnly fecha)
        {
            var desde = fecha.ToDateTime(TimeOnly.MinValue);
            var hasta = desde.AddDays(1);
            return _context.Pedidos.Count(p => p.FechaHora >= desde && p.FechaHora < hasta && p.TipoPedido == TipoPedido.Delivery);
        }

        public int GetCantidadPedidosRetiroDelDia(DateOnly fecha)
        {
            var desde = fecha.ToDateTime(TimeOnly.MinValue);
            var hasta = desde.AddDays(1);
            return _context.Pedidos.Count(p => p.FechaHora >= desde && p.FechaHora < hasta && p.TipoPedido == TipoPedido.Mostrador);
        }

        public int GetTotalFacturadoDelMes(int anio, int mes)
        {
            return _context.Pedidos
                .Where(p => p.FechaHora.Year == anio && p.FechaHora.Month == mes)
                .Sum(p => p.Total);
        }

        public int GetTotalPedidosDelMes(int anio, int mes)
        {
            return _context.Pedidos.Count(p => p.FechaHora.Year == anio && p.FechaHora.Month == mes);
        }

        public int GetTicketPromedioDelMes(int anio, int mes)
        {
            var cantidad = GetTotalPedidosDelMes(anio, mes);
            if (cantidad == 0)
            {
                return 0;
            }

            return GetTotalFacturadoDelMes(anio, mes) / cantidad;
        }

        public int GetCantidadPedidosDeliveryDelMes(int anio, int mes)
        {
            return _context.Pedidos.Count(p =>
                p.FechaHora.Year == anio &&
                p.FechaHora.Month == mes &&
                p.TipoPedido == TipoPedido.Delivery);
        }

        public int GetCantidadPedidosRetiroDelMes(int anio, int mes)
        {
            return _context.Pedidos.Count(p =>
                p.FechaHora.Year == anio &&
                p.FechaHora.Month == mes &&
                p.TipoPedido == TipoPedido.Mostrador);
        }
    }
}
