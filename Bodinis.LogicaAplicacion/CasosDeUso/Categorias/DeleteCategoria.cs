using Bodinis.LogicaAplicacion.DTOs.Categorias;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Categorias
{
    public class DeleteCategoria : ICUDelete<CategoriaDtoListado>
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
