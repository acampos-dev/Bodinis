using Bodinis.LogicaAplicacion.DTOs.Caja;
using Bodinis.LogicaAplicacion.Interfaces;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Caja
{
    public class GetCajaActual : ICUGetCajaActual
    {
        private readonly IRepositorioCaja _repoCaja;

        public GetCajaActual(IRepositorioCaja repoCaja)
        {
            _repoCaja = repoCaja;
        }

        public CajaDtoEstado Execute()
        {
            var caja = _repoCaja.GetCajaAbierta()
                ?? throw new CajaCerradaException("No hay una caja abierta");

            return CajaMapper.ToEstadoDto(caja);
        }
    }
}
