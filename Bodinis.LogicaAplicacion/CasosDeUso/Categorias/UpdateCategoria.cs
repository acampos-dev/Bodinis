using Bodinis.LogicaAplicacion.DTOs.Categorias;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Categorias
{
    public class UpdateCategoria : ICUUpdate<CategoriaDtoModificar>
    {
        private readonly IRepositorioCategoria _repoCategoria;

        public UpdateCategoria(IRepositorioCategoria repoCategoria)
        {
            _repoCategoria = repoCategoria;
        }

        public void Execute(int id, CategoriaDtoModificar dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
            {
                throw new DatosInvalidosException("El nombre de la categoria es obligatorio");
            }

            _repoCategoria.Update(id, CategoriaMapper.ToEntity(dto));
        }
    }
}
