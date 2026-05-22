using System.Security.Claims;
using ExpenseTracker.Web.Data;
using ExpenseTracker.Web.Models.Entities;
using ExpenseTracker.Web.Models.ViewModels;
using ExpenseTracker.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Web.Controllers;

public class AccountController : BaseController
{
    private readonly ApplicationDbContext _dbContext;
    private readonly PasswordService _passwordService;

    public AccountController(ApplicationDbContext dbContext, PasswordService passwordService)
    {
        _dbContext = dbContext;
        _passwordService = passwordService;
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Dashboard");
        }

        return View(new LoginViewModel());
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == viewModel.Email);
        if (user is null || !_passwordService.VerifyPassword(user, viewModel.Password))
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(viewModel);
        }

        await SignInAsync(user);
        return RedirectToAction("Index", "Dashboard");
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Dashboard");
        }

        return View(new RegisterViewModel());
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var exists = await _dbContext.Users.AnyAsync(x => x.Email == viewModel.Email);
        if (exists)
        {
            ModelState.AddModelError(nameof(viewModel.Email), "Email already exists.");
            return View(viewModel);
        }

        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            Name = viewModel.Name,
            Email = viewModel.Email,
            PasswordHash = _passwordService.HashPassword(viewModel.Password)
        };

        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        await SignInAsync(user);

        return RedirectToAction("Index", "Dashboard");
    }

    [Authorize]
    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View(new ChangePasswordViewModel());
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var user = await _dbContext.Users.FirstAsync(x => x.Id == CurrentUserId);
        if (!_passwordService.VerifyPassword(user, viewModel.CurrentPassword))
        {
            ModelState.AddModelError(nameof(viewModel.CurrentPassword), "Current password is incorrect.");
            return View(viewModel);
        }

        user.PasswordHash = _passwordService.HashPassword(viewModel.NewPassword);
        await _dbContext.SaveChangesAsync();
        TempData["Success"] = "Password updated successfully.";
        return RedirectToAction(nameof(ChangePassword));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    private async Task SignInAsync(AppUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Email, user.Email)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }
}
