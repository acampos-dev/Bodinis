using Bodinis.LogicaAplicacion.DTOs.Gastos;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Gastos
{
    public class RegistrarGasto : ICUAdd<GastoDtoAlta>
    {
        private readonly IRepositorioCaja _repoCaja;
        private readonly IRepositorioGasto _repoGasto;

        public RegistrarGasto(IRepositorioCaja repoCaja, IRepositorioGasto repoGasto)
        {
            _repoCaja = repoCaja;
            _repoGasto = repoGasto;
        }

        public void Execute(GastoDtoAlta dto)
        {
            var caja = _repoCaja.GetCajaAbierta()
                ?? throw new CajaCerradaException("No se puede registrar un gasto con la caja cerrada");

            var gasto = GastoMapper.ToEntity(dto, caja);
            _repoGasto.Add(gasto);
        }
    }
}
