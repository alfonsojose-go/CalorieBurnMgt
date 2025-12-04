using CalorieBurnMgt.Data;
using CalorieBurnMgt.Models;
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
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Challenge();
        }

        // Load user with calories
        var userWithCalories = await context.Users
            .Include(u => u.Calories)
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        if (userWithCalories == null)
            return NotFound();

        // ============================================
        // ⭐ NEW: Calculate remaining required distance
        // ============================================

        double caloriesPerKm = 80.0;

        // Total food-based required distance
        double totalCaloriesTaken = userWithCalories.Calories.Sum(c => c.CaloriesTaken);
        double requiredDistance = totalCaloriesTaken / caloriesPerKm;

        // Total distance actually burned through exercise
        double totalDistanceBurned = userWithCalories.Calories.Sum(c => c.DistanceTaken);

        // Remaining distance needed
        double remainingDistance = requiredDistance - totalDistanceBurned;

        if (remainingDistance < 0)
            remainingDistance = 0;

        // Read activity selection from query string
        string activity = Request.Query["activity"];

        // Default (Walking)
        double adjustedDistance = remainingDistance;

        // Jogging → half the walking distance
        if (activity == "Jogging")
        {
            adjustedDistance = remainingDistance / 2;
        }

        // Cycling → half of jogging (1/4 of walking)
        else if (activity == "Cycling")
        {
            adjustedDistance = remainingDistance / 4;
        }

        // Store for the view
        ViewBag.Activity = activity ?? "Walking";
        ViewBag.AdjustedDistance = adjustedDistance;


        ViewBag.RemainingDistance = remainingDistance;

        // TotalCalorieBalance already computed by your User model
        return View(userWithCalories);
    }
}
