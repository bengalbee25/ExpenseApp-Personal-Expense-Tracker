namespace ExpenseTracker.Web.Models.ViewModels;

public class MonthlyChartItemViewModel
{
    public string MonthLabel { get; set; } = string.Empty;
    public decimal Income { get; set; }
    public decimal Expense { get; set; }
}
