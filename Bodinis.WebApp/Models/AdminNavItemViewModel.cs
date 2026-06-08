namespace Bodinis.WebApp.Models
{
    public class AdminNavItemViewModel
    {
        public string Label { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Controller { get; set; } = "Admin";
        public string Action { get; set; } = "Index";
        public bool IsActive { get; set; }
        public bool IsDisabled { get; set; }
        public string DisabledReason { get; set; } = string.Empty;
    }
}
