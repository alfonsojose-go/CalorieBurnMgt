using CalorieBurnMgt.Data;
using CalorieBurnMgt.Models;
using CalorieBurnMgt.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

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
        // Get the current logged-in user
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return Challenge(); // Or redirect to login page
        }

        // Get user with included Calories data using the actual user ID
        var userWithCalories = await context.Users
            .Include(u => u.Calories)
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        if (userWithCalories == null)
        {
            return NotFound(); // Or handle appropriately
        }

        return View(userWithCalories);
    }
}
