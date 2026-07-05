using System.Globalization;

namespace Bodinis.WebApp.Helpers
{
    public static class MoneyFormatter
    {
        private static readonly NumberFormatInfo NumberFormat = new()
        {
            NumberGroupSeparator = ".",
            NumberDecimalSeparator = ",",
            NumberDecimalDigits = 0
        };

        public static string Format(int amount)
        {
            return $"$ {amount.ToString("N0", NumberFormat)}";
        }

        public static string Format(int? amount)
        {
            return amount.HasValue ? Format(amount.Value) : "$ 0";
        }
    }
}
