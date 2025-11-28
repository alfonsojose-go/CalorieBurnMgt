using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

public class ModulesController : Controller
{
    // Free module, accessible to all users
    public IActionResult Free()
    {
        return View(); // Returns Views/Modules/Free.cshtml
    }

    // Premium module, requires membership
    [Authorize] // Ensures user is logged in
    public IActionResult Premium()
    {
        bool isPremium = User.IsInRole("Premium"); // Or check database field
        if (!isPremium)
        {
            // Non-premium users redirected to upgrade prompt page
            return RedirectToAction("Upgrade");
        }

        return View(); // Returns Views/Modules/Premium.cshtml
    }

    // Prompt user to upgrade membership
    [Authorize]
    public IActionResult Upgrade()
    {
        return View(); // Returns Views/Modules/Upgrade.cshtml
    }
}