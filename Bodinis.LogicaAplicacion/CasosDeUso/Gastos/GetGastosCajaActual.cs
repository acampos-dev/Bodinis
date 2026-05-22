using Bodinis.LogicaAplicacion.DTOs.Gastos;
using Bodinis.LogicaAplicacion.Interfaces;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Gastos
{
    public class GetGastosCajaActual : ICUGetGastosPorCajaActual
    {
        private readonly IRepositorioCaja _repoCaja;
        private readonly IRepositorioGasto _repoGasto;

        public GetGastosCajaActual(IRepositorioCaja repoCaja, IRepositorioGasto repoGasto)
        {
            _repoCaja = repoCaja;
            _repoGasto = repoGasto;
        }

        public IEnumerable<GastoDtoListado> Execute()
        {
            var caja = _repoCaja.GetCajaAbierta()
                ?? throw new CajaCerradaException("No hay una caja abierta");

            return GastoMapper.ToListDto(_repoGasto.GetByCaja(caja.Id));
        }
    }
}
