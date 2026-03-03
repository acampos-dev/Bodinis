using Bodinis.LogicaAplicacion.DTOs.Categorias;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Categorias
{
    public class GetAllCategorias : ICUGetAll<CategoriaDtoListado>
    {
        private readonly IRepositorioCategoria _repoCategoria;

        public GetAllCategorias(IRepositorioCategoria repoCategoria)
        {
            _repoCategoria = repoCategoria;
        }

        public IEnumerable<CategoriaDtoListado> Execute()
        {
            var categorias = _repoCategoria.GetAll();
            return CategoriaMapper.ToListDto(categorias);
        }
    }
}