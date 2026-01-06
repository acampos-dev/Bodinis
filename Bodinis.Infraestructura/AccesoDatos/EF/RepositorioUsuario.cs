using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
using Bodinis.LogicaNegocio.Vo;
using Bodinis.Infraestructura.AccesoDatos.Excepciones;
using Microsoft.EntityFrameworkCore;

namespace Bodinis.Infraestructura.AccesoDatos.EF
{
    public class RepositorioUsuario : IRepositorioUsuario
    {
        private readonly BodinisContext _context;

        public RepositorioUsuario(BodinisContext context)
        {
            _context = context;
        }

        public Usuario GetByEmail(VoEmail email)
        {
            if (email == null)
                throw new BadRequestException("El email no puede ser nulo");

            return _context.Usuarios
                .FirstOrDefault(u => u.Email.Email == email.Email);
        }


        public Usuario GetById(int id)
        {
           if(id == 0) 
            {
                throw new BadRequestException("Esta id es incorrecto");
            }
           Usuario unUsuario = _context.Usuarios
                .FirstOrDefault(u => u.Id == id);
              if(unUsuario == null) 
            {
                throw new NotFoundException("No se encontro el usuario con la id solicitada");
            }
            else 
            {
                return unUsuario;
            }
        }

        public int Add(Usuario usuario)
        {
            if(usuario == null) 
            {
                throw new BadRequestException("El usuario no puede ser nulo");
            }
            usuario.Validar();
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return usuario.Id;
        }

        public void Update(int id, Usuario obj)
        {
            if (id==0)
            {
                throw new BadRequestException("El id no puede ser 0");
            }
            if(obj == null) 
            {
                throw new BadRequestException("El usuario no puede ser nulo");
            }
            Usuario usuario = GetById(id);
            _context.Usuarios.Update(usuario);
            _context.SaveChanges();
        }


    }
}
