using Bodinis.LogicaAplicacion.DTOs.Categorias;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Categorias
{
    public class AddCategoria : ICUAdd<CategoriaDtoAlta>
    {
        private readonly IRepositorioCategoria _repoCategoria;

        public AddCategoria(IRepositorioCategoria repoCategoria)
        {
            _repoCategoria = repoCategoria;
        }

        public void Execute(CategoriaDtoAlta dto)
        {
            var categoria = CategoriaMapper.ToEntity(dto);
            _repoCategoria.Add(categoria);
        }
    }
}
