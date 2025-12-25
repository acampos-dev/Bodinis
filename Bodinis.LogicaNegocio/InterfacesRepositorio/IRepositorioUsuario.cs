using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.Vo;
namespace Bodinis.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioUsuario:
        IRepositorioAdd<Usuario>,
        IRepositorioGetById<Usuario>,
        IRepositorioUpdate<Usuario>
    {
        Usuario GetByEmail(VoEmail email);
    }
}
