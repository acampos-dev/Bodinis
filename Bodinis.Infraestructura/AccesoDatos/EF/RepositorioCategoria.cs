using Bodinis.Infraestructura.AccesoDatos.Excepciones;
using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
using Microsoft.EntityFrameworkCore;

namespace Bodinis.Infraestructura.AccesoDatos.EF
{
    public class RepositorioCategoria: IRepositorioCategoria
    {
        private readonly BodinisContext _context;

        public RepositorioCategoria(BodinisContext context)
        {
            _context = context;
        }

        public void Add(Categoria categoria)
        {
            if(categoria == null)
            {
                throw new BadRequestException("La categoría no puede ser nula.");
            }
            _context.Categorias.Add(categoria);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var categoria = GetById(id);
            _context.Set<Categoria>().Remove(categoria);
            _context.SaveChanges();
        }

        public IEnumerable<Categoria> GetAll()
        {
            return _context.Set<Categoria>()
                .Include(c => c.Productos)
                .ToList();
        }

        public Categoria GetById(int id)
        {
            if (id <= 0)
                throw new BadRequestException("La id de categoria es inválida");

            var categoria = _context.Set<Categoria>()
                .Include(c => c.Productos)
                .FirstOrDefault(c => c.Id == id);

            if (categoria == null)
                throw new NotFoundException("No se encontró la categoría solicitada");

            return categoria;
        }

        public void Update(Categoria categoria)
        {
            if(categoria == null)
            {
                throw new BadRequestException("La categoría no puede ser nula.");
            }
            var categoriaExistente = GetById(categoria.Id);
            if (categoriaExistente == null)
                throw new NotFoundException("No se encontró la categoría a actualizar.");
            categoriaExistente.Nombre = categoria.Nombre;
            _context.SaveChanges();
        }


    }
}
