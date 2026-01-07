using Bodinis.LogicaNegocio.InterfacesLogicaNegocio;
using Bodinis.LogicaAplicacion.DTOs.Usuarios;
using Bodinis.LogicaAplicacion.Interfaces;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
using Bodinis.LogicaNegocio.Vo;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Usuarios
{
    public class LoginUsuario : ILogin<LoginDtoRequest>
    {
        private readonly IRepositorioUsuario _repositorioUsuario;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IPasswordHasher _passwordHasher;
            
        public LoginUsuario(
            IRepositorioUsuario repositorioUsuario,
            IJwtGenerator jwtGenerator,
            IPasswordHasher passwordHasher)
        {
            _repositorioUsuario = repositorioUsuario;
            _jwtGenerator = jwtGenerator;
            _passwordHasher = passwordHasher;
        }

        public string Execute(LoginDtoRequest request)
        {
            var email = new VoEmail(request.Email);

            var usuario = _repositorioUsuario.GetByEmail(email);
            if (usuario == null)
                throw new CredencialesInvalidasException();

            var passwordOk = _passwordHasher.Verify(
                request.Password,
                usuario.PasswordHash
            );

            usuario.ValidarLogin(passwordOk);

            return _jwtGenerator.GenerateToken(usuario);
        }



    }
}
