using Bodinis.LogicaAplicacion.DTOs.MetodoPago;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.MetodoPago
{
    public class GetMetodoPagoById : ICUGetById<MetodoPagoDtoListado>
    {
        private readonly IRepositorioMetodoPago _repoMetodoPago;

        public GetMetodoPagoById(IRepositorioMetodoPago repoMetodoPago)
        {
            _repoMetodoPago = repoMetodoPago;
        }

        public MetodoPagoDtoListado Execute(int id)
        {
            var metodoPago = _repoMetodoPago.GetById(id)
                ?? throw new DatosInvalidosException("Metodo de pago no encontrado");

            return MetodoPagoMapper.ToDto(metodoPago);
        }
    }
}
