using ExpenseTracker.Web.Data;
using ExpenseTracker.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Web.Controllers;

[Authorize]
public class DashboardController : BaseController
{
    private readonly ApplicationDbContext _dbContext;

    public DashboardController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var transactions = await _dbContext.Transactions
            .Where(x => x.UserId == CurrentUserId)
            .OrderByDescending(x => x.TransactionDate)
            .ToListAsync();

        var summary = new SummaryViewModel
        {
            Income = transactions.Where(x => x.Type == "income").Sum(x => x.Amount),
            Expense = transactions.Where(x => x.Type == "expense").Sum(x => x.Amount)
        };
        summary.Balance = summary.Income - summary.Expense;

        var monthly = transactions
            .GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month })
            .OrderBy(x => x.Key.Year).ThenBy(x => x.Key.Month)
            .Select(g => new MonthlyChartItemViewModel
            {
                MonthLabel = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy"),
                Income = g.Where(x => x.Type == "income").Sum(x => x.Amount),
                Expense = g.Where(x => x.Type == "expense").Sum(x => x.Amount)
            })
            .ToList();

        var pie = transactions
            .Where(x => x.Type == "expense")
            .GroupBy(x => x.Category)
            .Select(g => new CategoryChartItemViewModel
            {
                Category = g.Key,
                Amount = g.Sum(x => x.Amount)
            })
            .OrderByDescending(x => x.Amount)
            .ToList();

        var viewModel = new DashboardViewModel
        {
            Summary = summary,
            RecentTransactions = transactions.Take(5).ToList(),
            AllTransactions = transactions,
            MonthlyItems = monthly,
            ExpensePieItems = pie
        };

        return View(viewModel);
    }
}
