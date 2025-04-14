using _20232121_W2052838_PlanitGreen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace _20232121_W2052838_PlanitGreen.Filters
{
    public class RejectIfAdminAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var userRole = session.GetInt32("UserRole");

            if (userRole == (int)Role.Admin)
            {
                context.Result = new RedirectToActionResult("Dashboard", "Admin", null);
            }

            base.OnActionExecuting(context);
        }
    }
}
