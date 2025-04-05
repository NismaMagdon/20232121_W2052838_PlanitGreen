using Microsoft.AspNetCore.Mvc;

namespace _20232121_W2052838_PlanitGreen.Controllers
{
    public class AchievementController : Controller
    {
        public IActionResult Dashboard()
        {
            // Check if user is logged in by looking for UserID in the session
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                // Store the intended dashboard URL in session
                HttpContext.Session.SetString("ReturnUrl", Url.Action("Dashboard", "Achievement"));

                // Redirect to login page if not logged in
                return RedirectToAction("Login", "Account");
            }

            return View();
        }
    }
}
