using CalorieBurnMgt.Data;
using CalorieBurnMgt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class HomeController : Controller
{
    private readonly CalorieBurnDbContext context;
    private readonly UserManager<User> _userManager;

    public HomeController(CalorieBurnDbContext context, UserManager<User> userManager)
    {
        this.context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // ????????
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            // ????? ? ??????
            return RedirectToAction("Login", "Users");
        }

        // ????? Calories ??
        user = await context.Users
            .Include(u => u.Calories)
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        return View(user);
    }
}
