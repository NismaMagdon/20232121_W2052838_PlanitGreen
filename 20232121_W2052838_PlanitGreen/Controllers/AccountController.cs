using _20232121_W2052838_PlanitGreen.Managers;
using _20232121_W2052838_PlanitGreen.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _20232121_W2052838_PlanitGreen.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager userManager;
        private readonly Authenticator authenticator;

        public AccountController(UserManager userManager, Authenticator authenticator)
        {
            this.userManager = userManager;
            this.authenticator = authenticator;
        }
        
        //GET: Login page
        public IActionResult Login()
        {
            return View();
        }

        //POST: Handle login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = authenticator.AuthenticateUser(username, password);

            if (user == null) {
                ViewData["ErrorMessage"] = "Invalid username or password.";
                return View();
            }

            // Store the user ID and role in session to track login status
            HttpContext.Session.SetInt32("UserID", user.UserID);
            HttpContext.Session.SetInt32("UserRole", (int)user.Role);

            // Check if a return URL exists in session
            string? returnUrl = HttpContext.Session.GetString("ReturnUrl");

            if (!string.IsNullOrEmpty(returnUrl))
            {
                // Clear session variable and redirect to intended page
                HttpContext.Session.Remove("ReturnUrl");
                return Redirect(returnUrl);
            }

            if (user.Role == Role.Admin)
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            // Redirect to home page after successful login
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserID"); // Clear session
            HttpContext.Session.Remove("UserRole");
            return RedirectToAction("Index", "Home");
        }

        //GET: Register page
        public IActionResult Register()
        {
            return View();
        }

        //POST: Handle registration
        [HttpPost]
        public IActionResult Register(User user)
        {
            //Check if the username is taken
            if (authenticator.IsUsernameTaken(user.Username))
            {
                ViewData["ErrorMessage"] = "This username already exists.";
                return View();
            }

            user.Role = Role.Traveller;

            // Register the user using UserManager
            userManager.RegisterUser(user);

            // After successful registration, redirect to Login page
            return RedirectToAction("Login");

        }

        [HttpPost]
        public IActionResult RegisterAdmin(User user)
        {
            if (authenticator.IsUsernameTaken(user.Username))
            {
                TempData["ErrorMessage"] = "This username already exists.";
                return RedirectToAction("ManageAccounts", "Admin");
            }

            user.Role = Role.Admin;
            userManager.RegisterUser(user);

            TempData["SuccessMessage"] = "Admin account created successfully!";
            return RedirectToAction("ManageAccounts", "Admin");
        }
    }
}
