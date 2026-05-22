using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Web.Controllers;

public abstract class BaseController : Controller
{
    protected Guid CurrentUserId
    {
        get
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userId, out var id) ? id : Guid.Empty;
        }
    }
}
