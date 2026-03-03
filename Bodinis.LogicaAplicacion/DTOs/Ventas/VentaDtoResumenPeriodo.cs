using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bodinis.LogicaAplicacion.DTOs.Ventas
{
    public record VentaDtoResumenPeriodo(
                                   DateOnly Fecha,
                                   int CantidadVentas,
                                   int TotalVendido,
                                   int TicketPromedio
                                  ) 
    {
    }
}
