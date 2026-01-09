using Bodinis.LogicaNegocio.Entidades;

namespace Bodinis.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioCategoria:
            IRepositorioAdd<Categoria>,
            IRepositorioDelete<Categoria>,
            IRepositorioGetAll<Categoria>,
            IRepositorioGetById<Categoria>,
            IRepositorioUpdate<Categoria>
    {
    }
}
