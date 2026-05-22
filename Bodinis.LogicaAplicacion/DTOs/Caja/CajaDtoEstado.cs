namespace Bodinis.LogicaAplicacion.DTOs.Caja
{
    public record CajaDtoEstado(int CajaId,
                                DateTime FechaApertura,
                                DateTime? FechaCierre,
                                int MontoApertura,
                                int TotalVentas,
                                int TotalGastos,
                                int SaldoCalculado,
                                int MontoCierre,
                                bool EstaAbierta
        )
    {
    }
}
