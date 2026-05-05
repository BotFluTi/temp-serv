using Microsoft.EntityFrameworkCore;

namespace irrigation_system.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}

