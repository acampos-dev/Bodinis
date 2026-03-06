using Bodinis.LogicaNegocio.Entidades;
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

        public Caja Execute()
        {
            return _repoCaja.GetCajaAbierta();
        }
    }
}
