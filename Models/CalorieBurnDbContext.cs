using Microsoft.EntityFrameworkCore;


namespace CalorieBurnMgt.Models
{
    public class CalorieBurnDbContext : DbContext
    {
        public CalorieBurnDbContext(DbContextOptions<CalorieBurnDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Calorie> Calories { get; set; }

    }
}
