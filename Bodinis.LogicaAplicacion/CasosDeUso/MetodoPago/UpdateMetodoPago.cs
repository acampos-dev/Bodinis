using Bodinis.LogicaAplicacion.DTOs.MetodoPago;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.MetodoPago
{
    public class UpdateMetodoPago : ICUUpdate<MetodoPagoDtoModificar>
    {
        private readonly IRepositorioMetodoPago _repoMetodoPago;

        public UpdateMetodoPago(IRepositorioMetodoPago repoMetodoPago)
        {
            _repoMetodoPago = repoMetodoPago;
        }

        public void Execute(int id, MetodoPagoDtoModificar dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
            {
                throw new DatosInvalidosException("El nombre del metodo de pago es obligatorio");
            }

            var metodoPago = _repoMetodoPago.GetById(id)
                ?? throw new DatosInvalidosException("Metodo de pago no encontrado");

            metodoPago.Modificar(dto.Nombre, metodoPago.Activo);
            _repoMetodoPago.Update(metodoPago);
        }
    }
}
