
namespace Bodinis.LogicaNegocio.Entidades
{
    public class Venta
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public int TotalVenta { get; set; }
    

    public Venta() { } // Constructor para EF

    public Venta(DateTime fechaHora, int totalVenta)
    {
        FechaHora = fechaHora;
        TotalVenta = totalVenta;
    }
    

    }
}
