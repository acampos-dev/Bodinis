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
            return _context.Productos
                .Include(p => p.Categoria)
                .ToList();
        }

        public void Update(int id, Producto  obj) // Actualiza a partir de la id
        {
            if(obj == null) 
            {
                throw new BadRequestException("El producto no puede ser nulo");
            }
            Producto productoAActualizar = _context.Productos.FirstOrDefault(p => p.Id == id);

            if(productoAActualizar == null) 
            {
                throw new NotFoundException("No se encontro el producto con la id solicitada");
            }

            productoAActualizar.NombreProducto = new VoNombreProducto(obj.NombreProducto.Valor);
            productoAActualizar.Precio = new VoPrecio(obj.Precio.Valor);

        }

        public void Delete(int id)
        {
            Producto productoAEliminar = _context.Productos.FirstOrDefault(p => p.Id == id);
            if(productoAEliminar == null) 
            {
                throw new NotFoundException("No se encontro el producto con la id solicitada");
            }
            _context.Productos.Remove(productoAEliminar);
            _context.SaveChanges();
        }
    }
}
