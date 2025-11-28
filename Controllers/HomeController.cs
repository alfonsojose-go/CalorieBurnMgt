using CalorieBurnMgt.Data;
using CalorieBurnMgt.Models;
using CalorieBurnMgt.Data;
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

        public IActionResult Index(string userId)
        {
            var user = context.Users
                .Include(u => u.Calories)
                .FirstOrDefault(u => u.Id == userId);

            return View(user);
        }

    }
}
