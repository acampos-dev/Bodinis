using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
using Bodinis.LogicaNegocio.Vo;
using Bodinis.Infraestructura.AccesoDatos.Excepciones;
using Microsoft.EntityFrameworkCore;

namespace Bodinis.Infraestructura.AccesoDatos.EF
{
    public class RepositorioProducto: IRepositorioProducto
    {
        private readonly BodinisContext _context;

        public RepositorioProducto(BodinisContext context)
        {
            _context = context;
        }

        public void Add(Producto producto)
        {
            if(producto == null) 
            {
                throw new BadRequestException("El producto no puede ser nulo");
            }
            producto.Validar();
            _context.Productos.Add(producto);
            _context.SaveChanges();
        }

        public Producto GetById(int id)
        {
           if(id == 0) 
            {
                throw new BadRequestException("Esta id es incorrecto");
            }
           Producto unProducto = _context.Productos
                .FirstOrDefault(p => p.Id == id);
              if(unProducto == null) 
            {
                throw new NotFoundException("No se encontro el producto con la id solicitada");
            }
            else 
            {
                return unProducto;
            }
        }

        public IEnumerable<Producto> GetAll()
        {
            if(_context.Productos == null)
                {
                throw new NotFoundException("No se encontraron productos");
            }
            return _context.Productos
                .Include(p => p.Categoria)
                .ToList();
        }

        public void Update(int id, Producto obj) // Actualiza a partir de la id
        {
            if (obj == null)
            {
                throw new BadRequestException("El producto no puede ser nulo");
            }
            Producto productoAActualizar = _context.Productos.FirstOrDefault(p => p.Id == id);

            if (productoAActualizar == null)
            {
                throw new NotFoundException("No se encontro el producto con la id solicitada");
            }

            // Usar el método Modificar de la entidad Producto para actualizar los valores
            productoAActualizar.Modificar(
                obj.NombreProducto,
                obj.Precio,
                obj.Disponible,
                obj.Stock,
                obj.Categoria
            );

            _context.SaveChanges();
        }

        public IEnumerable<Producto> GetActivos()
        {
            if(_context.Productos == null)
            {
                throw new NotFoundException("No se encontraron productos");
            }
            return _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.Disponible)
                .ToList();
        }

    }
}
