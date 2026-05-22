namespace ExpenseTracker.Web.Models.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Type { get; set; } = "income";
    public decimal Amount { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public AppUser? User { get; set; }
}
