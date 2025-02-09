using Microsoft.EntityFrameworkCore;

namespace _20232121_W2052838_PlanitGreen.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }
    }
}
