using Bodinis.LogicaNegocio.Entidades;
namespace Bodinis.LogicaAplicacion.Interfaces
{
    public interface IJwtGenerator
    {
        string GenerateToken(Usuario usuario);
    }
}
