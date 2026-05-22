namespace Bodinis.LogicaNegocio.Entidades
{
    public class Caja
    {
        public int Id { get; set; }
        public DateTime FechaApertura { get; set; }
        public DateTime? FechaCierre { get; set; }
        public int MontoApertura { get; set; }
        public int MontoCierre { get; set; }
        public bool EstaAbierta { get; set; }
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
        public ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();

        public Caja() { } // Constructor para EF

        public Caja(DateTime fechaApertura, int montoApertura)
        {
            FechaApertura = fechaApertura;
            MontoApertura = montoApertura;
            EstaAbierta = true;
        }

        public int TotalVentas()
        {
            return Ventas?.Sum(v => v.TotalVenta) ?? 0;
        }

        public int TotalGastos()
        {
            return Gastos?.Sum(g => g.Monto) ?? 0;
        }

        public int SaldoCalculado()
        {
            return MontoApertura + TotalVentas() - TotalGastos();
        }

        public void Cerrar(DateTime fechaCierre, int montoCierre)
        {
            FechaCierre = fechaCierre;
            MontoCierre = montoCierre;
            EstaAbierta = false;
        }
    }
}
