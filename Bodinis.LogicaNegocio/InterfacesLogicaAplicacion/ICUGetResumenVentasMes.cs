using Bodinis.LogicaNegocio.ModelosCasosUso;

namespace Bodinis.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface ICUGetResumenVentasMes
    {
        ResumenVentasMes Execute(int anio, int mes);
    }
}
