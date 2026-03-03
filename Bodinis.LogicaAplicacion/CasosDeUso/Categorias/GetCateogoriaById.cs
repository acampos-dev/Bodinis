using Bodinis.LogicaAplicacion.DTOs.Categorias;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Categorias
{
    public class GetCategoriaById : ICUGetById<CategoriaDtoListado>
    {
        private readonly IRepositorioCategoria _repoCategoria;

        public GetCategoriaById(IRepositorioCategoria repoCategoria)
        {
            _repoCategoria = repoCategoria;
        }

        public CategoriaDtoListado Execute(int id)
        {
            var categoria = _repoCategoria.GetById(id);

            if (categoria == null)
            {
                throw new DatosInvalidosException("Categoría no encontrada");
            }

            return CategoriaMapper.ToDto(categoria);
        }
    }
}