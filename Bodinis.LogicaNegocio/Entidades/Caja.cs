namespace Bodinis.LogicaNegocio.Entidades
{
    public class Caja
    {
        public int Id { get; set; }
        public DateTime FechaApertura { get; set; }
        public DateTime? FechaCierre { get; set; }
        public int MontoApertura { get; set; }
        public int MontoCierre { get; set; }
        public ICollection<Venta> Ventas { get; set; }

        public Caja()
        {
        }

        public Caja(
            DateTime fechaApertura,
            int montoApertura,
            int montoCierre,
            ICollection<Venta> ventas,
            DateTime? fechaCierre = null)
        {
            FechaApertura = fechaApertura;
            FechaCierre = fechaCierre;
            MontoApertura = montoApertura;
            MontoCierre = montoCierre;
            Ventas = ventas ?? new List<Venta>();
        }
    }
}
