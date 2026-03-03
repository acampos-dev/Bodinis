using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Categorias
{
    public class DeleteCategoria : ICUDelete<Categoria>
    {
        private readonly IRepositorioCategoria _repoCategoria;

        public DeleteCategoria(IRepositorioCategoria repoCategoria)
        {
            _repoCategoria = repoCategoria;
        }

        public void Execute(int id)
        {
            _repoCategoria.Delete(id);
        }
    }
}