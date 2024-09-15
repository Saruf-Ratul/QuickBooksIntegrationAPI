// Models/BalanceSheet.cs
namespace QuickBooksIntegrationAPI.Models
{
    public class BalanceSheet
    {
        public string AccountName { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
    }
}