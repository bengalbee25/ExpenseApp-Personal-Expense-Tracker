using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Web.Models.ViewModels;

public class TransactionFormViewModel
{
    [Required]
    public string Type { get; set; } = "income";

    [Required, Range(0.01, 999999999)]
    public decimal Amount { get; set; }

    [Required, StringLength(100)]
    public string Category { get; set; } = string.Empty;

    [Required, DataType(DataType.Date)]
    public DateTime TransactionDate { get; set; } = DateTime.Today;

    [StringLength(500)]
    public string? Description { get; set; }
}
