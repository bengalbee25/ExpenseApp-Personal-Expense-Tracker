using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Web.Models.ViewModels;

public class EditTransactionViewModel : TransactionFormViewModel
{
    [Required]
    public Guid Id { get; set; }
}
