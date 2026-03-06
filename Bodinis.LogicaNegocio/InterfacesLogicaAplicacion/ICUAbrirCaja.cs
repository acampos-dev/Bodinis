using Bodinis.LogicaAplicacion.DTOs.Cajas;

namespace Bodinis.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface ICUAbrirCaja
    {
        CajaDtoResumen Execute(int montoApertura);
    }
}
