using _20232121_W2052838_PlanitGreen.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _20232121_W2052838_PlanitGreen.ViewComponents
{
    public class TourStyleDropdownViewComponent: ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public TourStyleDropdownViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tourStyles = await _context.TourStyle.ToListAsync();
            return View(tourStyles);
        }
    }
}
