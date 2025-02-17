using Microsoft.AspNetCore.Mvc;

namespace _20232121_W2052838_PlanitGreen.Controllers
{
    public class AccountController : Controller
    {

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
