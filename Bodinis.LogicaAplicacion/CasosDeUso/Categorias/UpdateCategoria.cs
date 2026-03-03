using Bodinis.LogicaAplicacion.DTOs.Categorias;
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
            var categoria = _repoCategoria.GetById(id);
            if (categoria == null)
            {
                throw new DatosInvalidosException("Categoría no encontrada");
            }

            categoria.Nombre = dto.nombre;
            _repoCategoria.Update(id, categoria);
        }
    }
}