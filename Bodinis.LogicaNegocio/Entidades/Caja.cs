

namespace Bodinis.LogicaNegocio.Entidades
{
    public class Caja
    {
        public int Id { get; set; }
        public DateTime FechaApertura { get; set; }
        public int MontoApertura { get; set; }
        public int MontoCierre { get; set; }
        public ICollection<Venta> Ventas { get; set; }
    

    public Caja() { } // Constructor para EF

    public Caja(DateTime fechaApertura, int montoApertura, int montocierre, ICollection<Venta> ventas)
    {
        FechaApertura = fechaApertura;
        MontoApertura = montoApertura;
        Ventas = new List<Venta>();
        }
    }


}