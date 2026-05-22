using Bodinis.LogicaAplicacion.DTOs.Caja;

namespace Bodinis.LogicaAplicacion.Interfaces
{
    public interface ICUCerrarCaja
    {
        CajaDtoEstado Execute(CajaDtoCerrar dto);
    }
}
