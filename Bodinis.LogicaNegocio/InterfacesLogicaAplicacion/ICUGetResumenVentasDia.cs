using Bodinis.LogicaNegocio.ModelosCasosUso;

namespace Bodinis.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface ICUGetResumenVentasDia
    {
        ResumenVentasDia Execute(DateOnly fecha);
    }
}
