using Bodinis.LogicaAplicacion.DTOs.Cajas;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Cajas
{
    public class GetCajaAbierta : ICUGetCajaAbierta
    {
        private readonly IRepositorioCaja _repoCaja;

        public GetCajaAbierta(IRepositorioCaja repoCaja)
        {
            _repoCaja = repoCaja;
        }

        public CajaDtoResumen Execute()
        {
            var caja = _repoCaja.GetCajaAbierta();
            return CajaReportesMapper.ToResumenDto(caja);
        }
    }
}
