using System.Diagnostics;
using CalorieBurnMgt.Models;
using CalorieBurnMgt.Data;
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

        public IActionResult Index(string userId)
        {
            var user = context.Users
                .Include(u => u.Calories)
                .FirstOrDefault(u => u.Id == userId);

            return View(user);
        }

    }
}