using Bodinis.LogicaAplicacion.DTOs.Cajas;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Cajas
{
    public class AbrirCaja : ICUAbrirCaja
    {
        private readonly IRepositorioCaja _repoCaja;

        public AbrirCaja(IRepositorioCaja repoCaja)
        {
            _repoCaja = repoCaja;
        }

        public CajaDtoResumen Execute(int montoApertura)
        {
            if (montoApertura < 0)
            {
                throw new DatosInvalidosException("El monto de apertura no puede ser negativo.");
            }

            var existeCajaAbierta = _repoCaja.GetAll().Any(c => c.FechaCierre == null);
            if (existeCajaAbierta)
            {
                throw new DatosInvalidosException("Ya existe una caja abierta.");
            }

            var caja = new Bodinis.LogicaNegocio.Entidades.Caja(
                DateTime.UtcNow,
                montoApertura,
                0,
                new List<Bodinis.LogicaNegocio.Entidades.Venta>());

            _repoCaja.Add(caja);
            return CajaReportesMapper.ToResumenDto(caja);
        }
    }
}
