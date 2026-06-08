namespace Bodinis.WebApp.Models
{
    public class AdminPageViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string ActiveSection { get; set; } = "Inicio";
        public string UserName { get; set; } = string.Empty;
        public string? ErrorMessage { get; set; }
        public bool CajaAbierta { get; set; }
        public CajaEstadoViewModel? CajaActual { get; set; }
        public IReadOnlyList<AdminNavItemViewModel> Navigation { get; set; } = Array.Empty<AdminNavItemViewModel>();
    }
}
