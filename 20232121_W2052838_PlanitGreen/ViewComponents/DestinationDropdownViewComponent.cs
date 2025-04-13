using _20232121_W2052838_PlanitGreen.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _20232121_W2052838_PlanitGreen.ViewComponents
{
    public class DestinationDropdownViewComponent: ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public DestinationDropdownViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var destinations = await _context.Destination.ToListAsync();
            return View(destinations);
        }
    }
}
