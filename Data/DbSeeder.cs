using ExpenseTracker.Web.Models.Entities;
using ExpenseTracker.Web.Services;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Web.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext dbContext, PasswordService passwordService)
    {
        if (await dbContext.Users.AnyAsync())
        {
            return;
        }

        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            Name = "Demo User",
            Email = "demo@example.com",
            PasswordHash = passwordService.HashPassword("demo123")
        };

        var transactions = new List<Transaction>
        {
            new() { Id = Guid.NewGuid(), UserId = user.Id, Type = "income", Amount = 50000, Category = "Salary", TransactionDate = new DateTime(2024, 1, 5), Description = "Monthly salary" },
            new() { Id = Guid.NewGuid(), UserId = user.Id, Type = "expense", Amount = 12000, Category = "Rent", TransactionDate = new DateTime(2024, 1, 8), Description = "Apartment rent" },
            new() { Id = Guid.NewGuid(), UserId = user.Id, Type = "expense", Amount = 3500, Category = "Groceries", TransactionDate = new DateTime(2024, 1, 12), Description = "Supermarket" },
            new() { Id = Guid.NewGuid(), UserId = user.Id, Type = "income", Amount = 8000, Category = "Freelance", TransactionDate = new DateTime(2024, 2, 2), Description = "Side project" },
            new() { Id = Guid.NewGuid(), UserId = user.Id, Type = "expense", Amount = 1500, Category = "Transport", TransactionDate = new DateTime(2024, 2, 6), Description = "Bus / ride sharing" },
            new() { Id = Guid.NewGuid(), UserId = user.Id, Type = "expense", Amount = 2200, Category = "Utilities", TransactionDate = new DateTime(2024, 2, 10), Description = "Electricity & water" },
            new() { Id = Guid.NewGuid(), UserId = user.Id, Type = "income", Amount = 51000, Category = "Salary", TransactionDate = new DateTime(2024, 3, 5), Description = "Monthly salary (increment)" },
            new() { Id = Guid.NewGuid(), UserId = user.Id, Type = "expense", Amount = 4000, Category = "Shopping", TransactionDate = new DateTime(2024, 3, 15), Description = "Clothes & accessories" },
            new() { Id = Guid.NewGuid(), UserId = user.Id, Type = "expense", Amount = 2000, Category = "Entertainment", TransactionDate = new DateTime(2024, 3, 20), Description = "Movies & snacks" }
        };

        await dbContext.Users.AddAsync(user);
        await dbContext.Transactions.AddRangeAsync(transactions);
        await dbContext.SaveChangesAsync();
    }
}
