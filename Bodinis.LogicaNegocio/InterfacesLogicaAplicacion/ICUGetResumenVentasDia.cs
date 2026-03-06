using Bodinis.LogicaAplicacion.DTOs.Ventas;

namespace Bodinis.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface ICUGetResumenVentasDia
    {
        VentaDtoResumenPeriodo Execute(DateOnly fecha);
    }
}
