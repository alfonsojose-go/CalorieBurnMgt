using CalorieBurnMgt.Data;
using CalorieBurnMgt.Helpers;
using CalorieBurnMgt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CalorieBurnMgt.Services;

namespace CalorieBurnMgt.Controllers
{
    public class CalorieController : Controller
    {
        private readonly CalorieBurnDbContext context;
        private readonly SessionHelper sessionHelper;
        private readonly ActivityCalculator calculator;

        // Add constructor to inject the database context
        public CalorieController(CalorieBurnDbContext context, SessionHelper sessionHelper, ActivityCalculator calculator)
        {
            this.context = context;
            this.sessionHelper = sessionHelper;
            this.calculator = calculator;
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
            string currentUserId = sessionHelper.GetCurrentUserId();

            if (DateConsumed == null || FoodName == null || FoodCalories == null ||
                DateConsumed.Length != FoodName.Length || FoodName.Length != FoodCalories.Length)
            {
                ModelState.AddModelError("", "Invalid form data");
                ViewBag.Foods = context.Foods.ToList();
                return View();
            }

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

                // OPTIONAL: Calculate required walking km
                double walkingRequired = calculator.CaloriesToWalkingKm(caloriesTaken);

                // Create new Calorie entry (distance NOT stored for food)
                var calorieEntry = new Calorie
                {
                    UserId = currentUserId,
                    FoodId = food.FoodId,
                    DateConsumed = dateConsumed,
                    CaloriesTaken = caloriesTaken,

                    // Food does not burn calories or distance
                    DateBurned = DateTime.MinValue,
                    CaloriesBurned = 0,
                    DistanceTaken = 0
                };

                context.Calories.Add(calorieEntry);
            }

            context.SaveChanges();

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
                string currentUserId = sessionHelper.GetCurrentUserId(); // Remove .ToString()

                // Validate that all arrays have the same length
                if (DateBurned == null || DistanceTaken == null || DateBurned.Length != DistanceTaken.Length)
                {
                    ModelState.AddModelError("", "Invalid form data");
                    return View();
                }

                // Find or create the "Exercise" food entry
                var exerciseFood = context.Foods
                    .FirstOrDefault(f => f.Name == "Exercise" || f.Name == "Distance Tracking");

                if (exerciseFood == null)
                {
                    // Create exercise food entry if it doesn't exist
                    exerciseFood = new Food
                    {
                        Name = "Exercise",
                        Calories = 0, // Don't assign calories for exercise food. Its calories are saved in Calorie Table, not in Food Table.
                    };
                    context.Foods.Add(exerciseFood);
                    context.SaveChanges(); // Save to get the FoodId
                }

            // Process each distance item
            for (int i = 0; i < DateBurned.Length; i++)
            {
                if (DistanceTaken[i] <= 0)
                    continue;

                if (!DateTime.TryParse(DateBurned[i], out DateTime dateBurned))
                    continue;

                // Read activity from dropdown
                string activity = ActivityType[i];

                // Convert to walking-equivalent km using the NEW calculator
                double walkingEq = calculator.ConvertToWalkingEquivalent((double)DistanceTaken[i], activity);

                // Convert walking-equivalent distance into calories burned
                int caloriesBurned = calculator.WalkingKmToCalories(walkingEq);

                // Create new Calorie entry
                var calorieEntry = new Calorie
                {
                    UserId = currentUserId,
                    FoodId = exerciseFood.FoodId,
                    DateConsumed = DateTime.MinValue,
                    DateBurned = dateBurned,
                    CaloriesTaken = 0,

                    // Store ONLY walking-equivalent km
                    DistanceTaken = (int)Math.Round(walkingEq),

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