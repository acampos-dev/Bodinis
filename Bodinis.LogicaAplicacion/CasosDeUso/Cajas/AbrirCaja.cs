using Bodinis.LogicaNegocio.Entidades;
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

        public Caja Execute(int montoApertura)
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

            var caja = new Caja(DateTime.UtcNow, montoApertura, 0, new List<Venta>());
            _repoCaja.Add(caja);
            return caja;
        }
    }
}
