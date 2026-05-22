using ExpenseTracker.Web.Models.Entities;

namespace ExpenseTracker.Web.Models.ViewModels;

public class DashboardViewModel
{
    public SummaryViewModel Summary { get; set; } = new();
    public List<Transaction> RecentTransactions { get; set; } = new();
    public List<Transaction> AllTransactions { get; set; } = new();
    public List<MonthlyChartItemViewModel> MonthlyItems { get; set; } = new();
    public List<CategoryChartItemViewModel> ExpensePieItems { get; set; } = new();
}
