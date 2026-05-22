using Microsoft.AspNetCore.Identity;
using ExpenseTracker.Web.Models.Entities;

namespace ExpenseTracker.Web.Services;

public class PasswordService
{
    private readonly PasswordHasher<AppUser> _hasher = new();

    public string HashPassword(string password)
    {
        return _hasher.HashPassword(new AppUser(), password);
    }

    public bool VerifyPassword(AppUser user, string password)
    {
        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;
    }
}
