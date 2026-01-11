using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Productos
{
    public class DesactivarProducto : ICUDeactivate
    {
        private readonly IRepositorioProducto _repoProducto;

        public DesactivarProducto(IRepositorioProducto repoProducto)
        {
            _repoProducto = repoProducto;
        }

        public void Execute(int id)
        {
            var producto = _repoProducto.GetById(id);
            if (producto == null)
                throw new DatosInvalidosException("Producto no existe");

            producto.Desactivar();

            _repoProducto.Update(id, producto);
        }
    }
}
