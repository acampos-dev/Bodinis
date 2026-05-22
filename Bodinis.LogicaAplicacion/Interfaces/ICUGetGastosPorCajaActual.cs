using Bodinis.LogicaAplicacion.DTOs.Gastos;

namespace Bodinis.LogicaAplicacion.Interfaces
{
    public interface ICUGetGastosPorCajaActual
    {
        IEnumerable<GastoDtoListado> Execute();
    }
}
