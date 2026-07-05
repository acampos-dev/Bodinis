using Bodinis.LogicaAplicacion.DTOs.Categorias;
using Bodinis.LogicaNegocio.Excepciones;
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
            var categoria = _repoCategoria.GetById(id);
            if (categoria.Productos?.Any() == true)
            {
                throw new DatosInvalidosException("No se puede eliminar una categoria con productos asociados. Cambia esos productos de categoria primero.");
            }

            _repoCategoria.Delete(id);
        }
    }
}
