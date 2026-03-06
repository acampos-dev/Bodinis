using Bodinis.LogicaAplicacion.DTOs.Cajas;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Cajas
{
    public class CerrarCaja : ICUCerrarCaja
    {
        private readonly IRepositorioCaja _repoCaja;

        public CerrarCaja(IRepositorioCaja repoCaja)
        {
            _repoCaja = repoCaja;
        }

        public CajaDtoResumen Execute()
        {
            var caja = _repoCaja.GetCajaAbierta();
            if (caja.FechaCierre != null)
            {
                throw new CajaCerradaException("La caja ya está cerrada.");
            }

            var totalVentas = caja.Ventas?.Sum(v => v.TotalVenta) ?? 0;
            caja.MontoCierre = caja.MontoApertura + totalVentas;
            caja.FechaCierre = DateTime.UtcNow;

            _repoCaja.Update(caja.Id, caja);
            return CajaReportesMapper.ToResumenDto(caja, caja.FechaCierre);
        }
    }
}
