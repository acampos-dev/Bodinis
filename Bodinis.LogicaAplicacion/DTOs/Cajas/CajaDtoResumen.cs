
namespace Bodinis.LogicaAplicacion.DTOs.Cajas
{
    public record CajaDtoResumen(
                                 int CajaId,
                                 DateTime FechaApertura,
                                 DateTime? FechaCierre,
                                 int MontoApertura,
                                 int TotalVentas,
                                 int MontoCierre
                                 )
    { }
    
}
