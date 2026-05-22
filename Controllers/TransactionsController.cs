using ExpenseTracker.Web.Data;
using ExpenseTracker.Web.Models.Entities;
using ExpenseTracker.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Web.Controllers;

[Authorize]
public class TransactionsController : BaseController
{
    private readonly ApplicationDbContext _dbContext;

    public TransactionsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> Income()
    {
        return View("Manage", await BuildManageViewModelAsync("income"));
    }

    [HttpGet]
    public async Task<IActionResult> Expenses()
    {
        return View("Manage", await BuildManageViewModelAsync("expense"));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(Prefix = "Form")] TransactionFormViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View("Manage", await BuildManageViewModelAsync(viewModel.Type, viewModel));
        }

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            UserId = CurrentUserId,
            Type = viewModel.Type,
            Amount = viewModel.Amount,
            Category = viewModel.Category,
            TransactionDate = viewModel.TransactionDate,
            Description = viewModel.Description
        };

        await _dbContext.Transactions.AddAsync(transaction);
        await _dbContext.SaveChangesAsync();
        TempData["Success"] = $"{Cap(viewModel.Type)} added successfully.";

        return RedirectToAction(viewModel.Type == "income" ? nameof(Income) : nameof(Expenses));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var item = await _dbContext.Transactions.FirstOrDefaultAsync(x => x.Id == id && x.UserId == CurrentUserId);
        if (item is null)
        {
            return NotFound();
        }

        var viewModel = new EditTransactionViewModel
        {
            Id = item.Id,
            Type = item.Type,
            Amount = item.Amount,
            Category = item.Category,
            TransactionDate = item.TransactionDate,
            Description = item.Description
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditTransactionViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var item = await _dbContext.Transactions.FirstOrDefaultAsync(x => x.Id == viewModel.Id && x.UserId == CurrentUserId);
        if (item is null)
        {
            return NotFound();
        }

        item.Type = viewModel.Type;
        item.Amount = viewModel.Amount;
        item.Category = viewModel.Category;
        item.TransactionDate = viewModel.TransactionDate;
        item.Description = viewModel.Description;

        await _dbContext.SaveChangesAsync();
        TempData["Success"] = "Transaction updated successfully.";
        return RedirectToAction(viewModel.Type == "income" ? nameof(Income) : nameof(Expenses));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, string type)
    {
        var item = await _dbContext.Transactions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.UserId == CurrentUserId);
        if (item is not null)
        {
            _dbContext.Transactions.Remove(item);
            await _dbContext.SaveChangesAsync();
            TempData["Success"] = "Transaction deleted successfully.";
        }

        return RedirectToAction(type == "expense" ? nameof(Expenses) : nameof(Income));
    }

    [HttpGet]
    public async Task<IActionResult> Search(string query = "")
    {
        var transactions = await _dbContext.Transactions
            .Where(x => x.UserId == CurrentUserId)
            .OrderByDescending(x => x.TransactionDate)
            .ToListAsync();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var normalized = query.Trim().ToLower();
            transactions = transactions.Where(t =>
                    t.TransactionDate.ToString("dd/MM/yyyy").ToLower().Contains(normalized)
                    || t.Type.ToLower().Contains(normalized)
                    || t.Category.ToLower().Contains(normalized)
                    || t.Amount.ToString("0.##").Contains(normalized)
                    || (t.Description ?? string.Empty).ToLower().Contains(normalized))
                .ToList();
        }

        return View(new SearchTransactionsViewModel
        {
            Query = query,
            Transactions = transactions
        });
    }

    [HttpGet]
    public async Task<IActionResult> Report()
    {
        var dashboardController = new DashboardController(_dbContext);
        dashboardController.ControllerContext = ControllerContext;
        var result = await dashboardController.Index() as ViewResult;
        return View(result?.Model as DashboardViewModel ?? new DashboardViewModel());
    }

    private async Task<ManageTransactionsPageViewModel> BuildManageViewModelAsync(string type, TransactionFormViewModel? form = null)
    {
        var items = await _dbContext.Transactions
            .Where(x => x.UserId == CurrentUserId && x.Type == type)
            .OrderByDescending(x => x.TransactionDate)
            .ToListAsync();

        return new ManageTransactionsPageViewModel
        {
            PageType = type,
            Title = type == "income" ? "Income" : "Expenses",
            Form = form ?? new TransactionFormViewModel { Type = type, TransactionDate = DateTime.Today },
            Transactions = items
        };
    }

    private static string Cap(string value) => char.ToUpper(value[0]) + value[1..];
}

public class ManageTransactionsPageViewModel
{
    public string PageType { get; set; } = "income";
    public string Title { get; set; } = string.Empty;
    public TransactionFormViewModel Form { get; set; } = new();
    public List<Transaction> Transactions { get; set; } = new();
}
