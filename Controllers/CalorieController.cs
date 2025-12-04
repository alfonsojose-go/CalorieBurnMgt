using CalorieBurnMgt.Data;
using CalorieBurnMgt.Helpers;
using CalorieBurnMgt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalorieBurnMgt.Controllers
{
    public class CalorieController : Controller
    {
        private readonly CalorieBurnDbContext context;
        private readonly SessionHelper sessionHelper;

        // Add constructor to inject the database context
        public CalorieController(CalorieBurnDbContext context, SessionHelper sessionHelper)
        {
            this.context = context;
            this.sessionHelper = sessionHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddCalorie()
        {
            // Pass the list of foods to the view
            ViewBag.Foods = context.Foods.OrderBy(f => f.Name).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AddCalorie(string[] DateConsumed, string[] FoodName, int[] FoodCalories)
        {
            // Get the current user's ID
            string currentUserId = sessionHelper.GetCurrentUserId();

            // Validate form
            if (DateConsumed == null || FoodName == null || FoodCalories == null ||
                DateConsumed.Length != FoodName.Length || FoodName.Length != FoodCalories.Length)
            {
                ModelState.AddModelError("", "Invalid form data");
                ViewBag.Foods = context.Foods.ToList();
                return View();
            }

            double totalDistanceToRun = 0;

            // Process each food item entry
            for (int i = 0; i < DateConsumed.Length; i++)
            {
                if (string.IsNullOrEmpty(FoodName[i]))
                    continue;

                var food = context.Foods.FirstOrDefault(f => f.Name == FoodName[i]);
                if (food == null)
                    continue;

                if (!DateTime.TryParse(DateConsumed[i], out DateTime dateConsumed))
                    continue;

                int caloriesTaken = FoodCalories[i];

                // Calculate distance needed to burn eaten calories
                double caloriesPerKm = 80.0;
                double distanceToRun = caloriesTaken / caloriesPerKm;
                totalDistanceToRun += distanceToRun;

                // CREATE CALORIE ENTRY FOR FOOD (NO CaloriesBurned)
                var calorieEntry = new Calorie
                {
                    UserId = currentUserId,
                    FoodId = food.FoodId,
                    DateConsumed = dateConsumed,
                    CaloriesTaken = caloriesTaken,

                    // FIX: Food should NOT burn calories
                    CaloriesBurned = 0,

                    DateBurned = DateTime.MinValue,
                    DistanceTaken = 0
                };

                context.Calories.Add(calorieEntry);
            }

            context.SaveChanges();

            // Store distance in TempData as string
            TempData["DistanceToRun"] = totalDistanceToRun.ToString();

            TempData["SuccessMessage"] = "Calories added successfully!";
            return RedirectToAction("Index", "Home");
        }

        // GET: Display the Add Distance form
        [HttpGet]
        public IActionResult SubtractDistance()
        {
            return View();
        }

        // POST: Handle distance form submission
        [HttpPost]
        public IActionResult SubtractDistance(string[] DateBurned, decimal[] DistanceTaken, string[] ActivityType)
        {
            // Get the current user's ID using SessionHelper
            string currentUserId = sessionHelper.GetCurrentUserId();

            // Validate that all arrays have the same length
            if (DateBurned == null || DistanceTaken == null || ActivityType == null ||
                DateBurned.Length != DistanceTaken.Length || ActivityType.Length != DistanceTaken.Length)
            {
                ModelState.AddModelError("", "Invalid form data");
                return View();
            }

            // Find or create the "Exercise" food entry
            var exerciseFood = context.Foods
                .FirstOrDefault(f => f.Name == "Exercise" || f.Name == "Distance Tracking");

            if (exerciseFood == null)
            {
                exerciseFood = new Food
                {
                    Name = "Exercise",
                    Calories = 0, // Exercise calories are stored in Calorie table
                };
                context.Foods.Add(exerciseFood);
                context.SaveChanges();
            }

            // Process each distance item
            for (int i = 0; i < DateBurned.Length; i++)
            {
                // Skip if distance is not provided or is zero/negative
                if (DistanceTaken[i] <= 0)
                    continue;

                // Parse the date
                if (!DateTime.TryParse(DateBurned[i], out DateTime dateBurned))
                    continue;

                // --- Convert to walking-equivalent distance ---
                double rawDistance = (double)DistanceTaken[i]; // what user entered
                string activity = ActivityType[i];

                double walkingEquivalent = rawDistance; // default: walking

                if (string.Equals(activity, "Jogging", StringComparison.OrdinalIgnoreCase))
                {
                    // Jogging is twice as effective as walking
                    walkingEquivalent = rawDistance * 2;
                }
                else if (string.Equals(activity, "Cycling", StringComparison.OrdinalIgnoreCase))
                {
                    // Cycling is 4x as effective as walking
                    walkingEquivalent = rawDistance * 4;
                }

                // Calculate calories burned using walking-equivalent distance
                int caloriesBurned = (int)(walkingEquivalent * 62);

                // Create new Calorie entry
                var calorieEntry = new Calorie
                {
                    UserId = currentUserId,
                    FoodId = exerciseFood.FoodId,
                    DateConsumed = DateTime.MinValue, // Not applicable for distance entries
                    DateBurned = dateBurned,
                    CaloriesTaken = 0, // No calories consumed for distance entries

                    // Store walking-equivalent distance (so HomeController math stays consistent)
                    DistanceTaken = (int)Math.Round(walkingEquivalent),

                    CaloriesBurned = caloriesBurned
                };

                context.Calories.Add(calorieEntry);
            }

            // Save all changes to database
            context.SaveChanges();

            TempData["SuccessMessage"] = "Distance and calories burned added successfully!";
            return RedirectToAction("Index", "Home");
        }
    }
}
