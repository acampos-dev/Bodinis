namespace Bodinis.WebApp.Models
{
    public class AdminCajaViewModel : AdminPageViewModel
    {
        public IReadOnlyList<GastoAdminViewModel> Gastos { get; set; } = Array.Empty<GastoAdminViewModel>();
        public string? SuccessMessage { get; set; }
        public int CantidadGastos => Gastos.Count;
        public int TotalGastosCaja => Gastos.Sum(gasto => gasto.Monto);
    }
}
