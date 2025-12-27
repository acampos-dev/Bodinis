using Bodinis.Infraestructura.AccesoDatos.EF.Config;
using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
using Bodinis.LogicaNegocio.Vo;
using Microsoft.EntityFrameworkCore;

namespace Bodinis.Infraestructura.Repositorios
{
    public class RepositorioUsuario : IRepositorioUsuario
    {
        private readonly BodinisDbContext _context;

        public RepositorioUsuario(BodinisDbContext context)
        {
            _context = context;
        }

        public Usuario GetByEmail(Email email)
        {
            return _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefault(u => u.Email == email);
        }

        public Usuario GetById(int id)
        {
            return _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefault(u => u.Id == id);
        }

        public int Add(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return usuario.Id;
        }
    }
}
