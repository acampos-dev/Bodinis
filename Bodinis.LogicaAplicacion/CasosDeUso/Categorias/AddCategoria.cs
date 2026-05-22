using Bodinis.LogicaAplicacion.DTOs.Categorias;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.Excepciones;
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
            if (string.IsNullOrWhiteSpace(dto.Nombre))
            {
                throw new DatosInvalidosException("El nombre de la categoria es obligatorio");
            }

            _repoCategoria.Add(CategoriaMapper.ToEntity(dto));
        }
    }
}
