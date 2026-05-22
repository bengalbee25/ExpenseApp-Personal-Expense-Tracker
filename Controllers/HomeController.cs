using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Web.Controllers;

public class HomeController : Controller
{
    [Route("/Home/Error")]
    public IActionResult Error() => View();
}
