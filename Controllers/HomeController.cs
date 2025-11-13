using System.Diagnostics;
using CalorieBurnMgt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalorieBurnMgt.Controllers
{
    public class HomeController : Controller
    {
        private readonly CalorieBurnDbContext context;

        public HomeController(CalorieBurnDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            // Get the first user from database (adjust as needed)
            var user = context.Users
                .Include(u => u.Calorie) // Include related Calorie data if needed
                .FirstOrDefault();

            return View(user);
        }
    }
}