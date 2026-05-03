namespace Bodinis.LogicaAplicacion.DTOs.Caja
{
    public record CajaDtoEstado(int CajaId, 
                                DateTime FechaApertura,
                                int MontoApertura,
                                bool EstaAbierta
        )
    {
    }
}
