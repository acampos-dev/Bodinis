using Bodinis.LogicaAplicacion.DTOs.Caja;
using Bodinis.LogicaAplicacion.Interfaces;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Caja
{
    public class CerrarCaja : ICUCerrarCaja
    {
        private readonly IRepositorioCaja _repoCaja;

        public CerrarCaja(IRepositorioCaja repoCaja)
        {
            _repoCaja = repoCaja;
        }

        public CajaDtoEstado Execute(CajaDtoCerrar dto)
        {
            var caja = _repoCaja.GetCajaAbierta()
                ?? throw new CajaCerradaException("No hay una caja abierta para cerrar");

            if (dto.MontoFinal < 0)
            {
                throw new DatosInvalidosException("El monto final no puede ser negativo");
            }

            caja.Cerrar(DateTime.Now, dto.MontoFinal);
            _repoCaja.CerrarCaja(caja);

            return CajaMapper.ToEstadoDto(caja);
        }
    }
}
