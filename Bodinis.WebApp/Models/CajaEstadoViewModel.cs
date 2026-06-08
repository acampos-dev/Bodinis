namespace Bodinis.WebApp.Models
{
    public class CajaEstadoViewModel
    {
        public int CajaId { get; set; }
        public DateTime FechaApertura { get; set; }
        public DateTime? FechaCierre { get; set; }
        public int MontoApertura { get; set; }
        public int TotalVentas { get; set; }
        public int TotalGastos { get; set; }
        public int SaldoCalculado { get; set; }
        public int MontoCierre { get; set; }
        public bool EstaAbierta { get; set; }
    }
}
