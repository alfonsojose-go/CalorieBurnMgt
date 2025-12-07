using CalorieBurnMgt.Data;
using CalorieBurnMgt.Models;
using CalorieBurnMgt.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class HomeController : Controller
{
    private readonly CalorieBurnDbContext context;
    private readonly UserManager<User> _userManager;
    private readonly ActivityCalculator calculator;

    public HomeController(
        CalorieBurnDbContext context,
        UserManager<User> userManager,
        ActivityCalculator calculator)
    {
        this.context = context;
        _userManager = userManager;
        this.calculator = calculator;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Challenge();

        var userWithCalories = await context.Users
            .Include(u => u.Calories)
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        if (userWithCalories == null)
            return NotFound();

        // ✅ Use the read-only property – no assignment
        int balance = userWithCalories.TotalCalorieBalance;

        // Safety: no negative balance
        if (balance < 0)
            balance = 0;

        // Base walking km needed (80 cal = 1 km)
        double walkingRequired = calculator.CaloriesToWalkingKm(balance);
        walkingRequired = Math.Max(0, walkingRequired);

        // Jogging = half distance, Cycling = quarter
        double joggingRequired = walkingRequired / 2;
        double cyclingRequired = walkingRequired / 4;

        // Round for display
        ViewBag.RequiredWalking = Math.Round(walkingRequired, 2);
        ViewBag.RequiredJogging = Math.Round(joggingRequired, 2);
        ViewBag.RequiredCycling = Math.Round(cyclingRequired, 2);

        return View(userWithCalories);
    }
}
