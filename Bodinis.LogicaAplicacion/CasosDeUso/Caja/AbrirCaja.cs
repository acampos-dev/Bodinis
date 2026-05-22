using Bodinis.LogicaAplicacion.DTOs.Caja;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Caja
{
    public class AbrirCaja : ICUAdd<CajaDtoAbrir>
    {
        private readonly IRepositorioCaja _repoCaja;

        public AbrirCaja(IRepositorioCaja repoCaja)
        {
            _repoCaja = repoCaja;
        }

        public void Execute(CajaDtoAbrir dto)
        {
            if (_repoCaja.GetCajaAbierta() != null)
            {
                throw new DatosInvalidosException("Ya existe una caja abierta");
            }

            if (dto.MontoInicial < 0)
            {
                throw new DatosInvalidosException("El monto inicial no puede ser negativo");
            }

            var caja = new Bodinis.LogicaNegocio.Entidades.Caja(DateTime.Now, dto.MontoInicial);
            _repoCaja.AbrirCaja(caja);
        }
    }
}
