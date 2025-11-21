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

        public IActionResult Index(int userId = 6) // Pass userId as parameter
        {
            var user = context.Users
                .Include(u => u.Calories)
                .FirstOrDefault(u => u.UserId == userId);
            return View(user);
        }
    }
}