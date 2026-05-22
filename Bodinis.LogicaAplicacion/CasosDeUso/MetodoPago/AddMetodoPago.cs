using Bodinis.LogicaAplicacion.DTOs.MetodoPago;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.MetodoPago
{
    public class AddMetodoPago : ICUAdd<MetodoPagoDtoAlta>
    {
        private readonly IRepositorioMetodoPago _repoMetodoPago;

        public AddMetodoPago(IRepositorioMetodoPago repoMetodoPago)
        {
            _repoMetodoPago = repoMetodoPago;
        }

        public void Execute(MetodoPagoDtoAlta dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
            {
                throw new DatosInvalidosException("El nombre del metodo de pago es obligatorio");
            }

            _repoMetodoPago.Add(MetodoPagoMapper.ToEntity(dto));
        }
    }
}
