using ExpenseTracker.Web.Models.Entities;

namespace ExpenseTracker.Web.Models.ViewModels;

public class SearchTransactionsViewModel
{
    public string Query { get; set; } = string.Empty;
    public List<Transaction> Transactions { get; set; } = new();
}
