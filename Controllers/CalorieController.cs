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
            // Get the current user's ID (adjust this based on your authentication setup)
            string currentUserId = "6"; //sessionHelper.GetCurrentUserId(); // You'll need to implement this method

            // Validate that all arrays have the same length
            if (DateConsumed == null || FoodName == null || FoodCalories == null ||
                DateConsumed.Length != FoodName.Length || FoodName.Length != FoodCalories.Length)
            {
                ModelState.AddModelError("", "Invalid form data");
                ViewBag.Foods = context.Foods.ToList();
                return View();
            }

            // Process each food item
            for (int i = 0; i < DateConsumed.Length; i++)
            {
                // Skip if no food was selected
                if (string.IsNullOrEmpty(FoodName[i]))
                    continue;

                // Find the food ID by name
                var food = context.Foods.FirstOrDefault(f => f.Name == FoodName[i]);
                if (food == null)
                    continue;

                // Parse the date
                if (!DateTime.TryParse(DateConsumed[i], out DateTime dateConsumed))
                    continue;

                // Create new Calorie entry
                var calorieEntry = new Calorie
                {
                    UserId = currentUserId,
                    FoodId = food.FoodId, // Assuming your Food model has FoodId property
                    DateConsumed = dateConsumed,
                    CaloriesTaken = FoodCalories[i],
                    DateBurned = DateTime.MinValue, // Set default or leave for later
                    DistanceTaken = 0, // Set default or leave for later
                    CaloriesBurned = 0 // Set default or leave for later
                };

                context.Calories.Add(calorieEntry);
            }

            // Save all changes to database
            context.SaveChanges();

            TempData["SuccessMessage"] = "Calories added successfully!";
            return RedirectToAction("Index", "Home");
        }

        // GET: Display the Add Distance form
        [HttpGet]
        public IActionResult AddDistance()
        {
            return View();
        }

        // POST: Handle distance form submission
        [HttpPost]
        public IActionResult AddDistance(string[] DateBurned, decimal[] DistanceTaken)
        {
            // Get the current user's ID using SessionHelper
            string currentUserId = "6"; ; // sessionHelper.GetCurrentUserId();

            // Validate that all arrays have the same length
            if (DateBurned == null || DistanceTaken == null || DateBurned.Length != DistanceTaken.Length)
            {
                ModelState.AddModelError("", "Invalid form data");
                return View();
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

                // Calculate calories burned using David's formula
                // Assuming formula: CaloriesBurned = DistanceTaken * 62 (adjust this based on actual formula)
                int caloriesBurned = (int)(DistanceTaken[i] * 62);

                // Create new Calorie entry
                var calorieEntry = new Calorie
                {
                    UserId = currentUserId,
                    FoodId = 0, // No food associated with distance entries
                    DateConsumed = DateTime.MinValue, // Not applicable for distance entries
                    DateBurned = dateBurned,
                    CaloriesTaken = 0, // No calories consumed for distance entries
                    DistanceTaken = (int)DistanceTaken[i], // Convert to int if your model uses int
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