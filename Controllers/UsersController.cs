using System.ComponentModel.DataAnnotations;
using CalorieBurnMgt.Data;
using CalorieBurnMgt.DTOs;
using CalorieBurnMgt.Models;
using CalorieBurnMgt.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;

public class UsersController : Controller
{
    private readonly CalorieBurnDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IEmailService _emailService;

    public UsersController(CalorieBurnDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register( CalorieBurnMgt.DTOs.RegisterRequest dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var user = new User
        {
            UserName = dto.UserName,
            FullName = dto.FullName,
            Age = dto.Age,
            Weight = dto.Weight,
            Email = dto.Email, // ✅ Must include
            Height = dto.Height
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (result.Succeeded)
            return RedirectToAction("Login");

        foreach (var err in result.Errors)
            ModelState.AddModelError("", err.Description);

        return View(dto);
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(CalorieBurnMgt.DTOs.LoginRequest dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var user = await _userManager.FindByNameAsync(dto.UserName);

        // 🔴 Username does not exist
        if (user == null)
        {
            ModelState.AddModelError("UserName", "Username does not exist.");
            return View(dto);
        }

        // 🔴 Password incorrect
        var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!passwordCheck.Succeeded)
        {
            ModelState.AddModelError("Password", "Password is incorrect.");
            return View(dto);
        }

        // Login success
        await _signInManager.SignInAsync(user, false);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login");

        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult ResetPassword() => View();

    

    [HttpGet]
    public async Task<IActionResult> Membership()
    {
        // 🔹 Get current logged-in username
        var userName = User.Identity?.Name;
        if (string.IsNullOrEmpty(userName))
        {
            // User not logged in, redirect to login page
            return RedirectToAction("Login");
        }

        // 🔹 Get user from database
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        if (user == null)
        {
            // User not found, redirect to login page
            return RedirectToAction("Login");
        }

        // 🔹 Return membership page with user object
        return View(user);
    }

    // Upgrade membership → create Stripe Checkout session
    [HttpPost]
    public IActionResult UpgradeMembership()
    {
        var domain = "https://localhost:7022"; // Change to your domain
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = 1000, // 10 USD
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Premium Membership"
                        }
                    },
                    Quantity = 1
                }
            },
            Mode = "payment",
            SuccessUrl = domain + "/Users/StripeSuccess",
            CancelUrl = domain + "/Users/Membership"
        };

        var service = new SessionService();
        Session session = service.Create(options);

        return Redirect(session.Url);
    }

    [HttpGet]
    public async Task<IActionResult> StripeSuccess()
    {
        // 1️⃣ Check if user is logged in
        if (User?.Identity == null || string.IsNullOrEmpty(User.Identity.Name))
        {
            TempData["Message"] = "You must be logged in to upgrade membership.";
            return RedirectToAction("Login");
        }

        try
        {
            // 2️⃣ Find user
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (user == null)
            {
                TempData["Message"] = "User not found.";
                return RedirectToAction("Login");
            }

            // 3️⃣ Update membership
            user.IsPremium = true;
            user.PremiumExpireDate = DateTime.UtcNow.AddMonths(1);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Membership upgraded successfully!";
        }
        catch (Exception ex)
        {
            // 4️⃣ Error handling
            TempData["Message"] = "An error occurred while upgrading membership.";
        }

        return RedirectToAction("Membership");
    }

    [HttpPost]
    public async Task<IActionResult> CancelMembership()
    {
        var userName = User.Identity?.Name;
        if (string.IsNullOrEmpty(userName))
            return RedirectToAction("Login");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        if (user == null)
            return RedirectToAction("Login");

        if (!user.IsPremium)
        {
            TempData["Message"] = "You are not currently a premium member.";
            return RedirectToAction("Membership");
        }

        // Cancel membership
        user.IsPremium = false;
        user.PremiumExpireDate = null;
        await _context.SaveChangesAsync();

        TempData["Message"] = "Membership cancelled successfully!";
        return RedirectToAction("Membership");
    }

    // === Forgot Password feature - start ===

    [HttpGet]
    public IActionResult ForgotPassword() => View();

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest dto)
    {
        if (!ModelState.IsValid) return View(dto);

        string resetLink = string.Empty;
        string message = string.Empty;

        // Find user by email
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user != null)
        {
            // User exists → generate real token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = System.Web.HttpUtility.UrlEncode(token);
            resetLink = Url.Action("ResetPasswordWithToken", "Users",
                new { token = encodedToken, email = user.Email },
                protocol: HttpContext.Request.Scheme);

            message = $"Email exists ✅, generated real reset link:";
        }
        else
        {
            // User does not exist → generate fake token for demo
            var fakeToken = "demo-token";
            resetLink = Url.Action("ResetPasswordWithToken", "Users",
                new { token = fakeToken, email = dto.Email },
                protocol: HttpContext.Request.Scheme);

            message = $"Email does not exist ❌, generated demo reset link:";
        }

        TempData["Message"] = message;
        TempData["DemoResetLink"] = resetLink;
        return View("ForgotPasswordConfirmation");
    }

    [HttpGet]
    public IActionResult ForgotPasswordConfirmation() => View();

    [HttpGet]
    public IActionResult ResetPasswordWithToken(string token, string email)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
        {
            TempData["ErrorMessage"] = "Invalid reset link";
            return RedirectToAction("Login");
        }

        var dto = new ResetPasswordRequestDto
        {
            Token = token,
            Email = email
        };

        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> ResetPasswordWithToken(ResetPasswordRequestDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            // Do not reveal user existence
            ModelState.AddModelError("", "Reset failed. Please check if the link is correct.");
            return View(dto);
        }

        // URL decode token
        var decodedToken = System.Web.HttpUtility.UrlDecode(dto.Token);

        var result = await _userManager.ResetPasswordAsync(user, decodedToken, dto.NewPassword);

        if (result.Succeeded)
        {
            TempData["Message"] = "Password reset successfully! Please log in with your new password.";
            return RedirectToAction("Login");
        }

        foreach (var err in result.Errors)
            ModelState.AddModelError("", err.Description);

        return View(dto);
    }

    // === Forgot Password feature - end ===
}
