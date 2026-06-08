namespace Bodinis.WebApp.Models
{
    public class GastoAdminViewModel
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int Monto { get; set; }
        public string? Categoria { get; set; }
        public int CajaId { get; set; }
    }
}
