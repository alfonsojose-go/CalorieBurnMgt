using CalorieBurnMgt.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CalorieBurnMgt.Data
{
    public class CalorieBurnDbContext : IdentityDbContext<User>
    {
        public CalorieBurnDbContext(DbContextOptions<CalorieBurnDbContext> options)
            : base(options)
        {
        }

        // 保留原有 DbSet
        public DbSet<User> Users { get; set; } // Identity 会自动管理 User 表，可以保留或删除视情况
        public DbSet<Food> Foods { get; set; }
        public DbSet<Calorie> Calories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // 必须调用 base，Identity 才能正确生成表结构

            // 保留种子数据
            modelBuilder.Entity<Food>().HasData(
                new Food { FoodId = 1, Name = "Apple (medium)", Calories = 95 },
                new Food { FoodId = 2, Name = "Banana (medium)", Calories = 105 },
                new Food { FoodId = 3, Name = "White bread (slice)", Calories = 80 },
                new Food { FoodId = 4, Name = "Brown rice (cooked, 1 cup)", Calories = 215 },
                new Food { FoodId = 5, Name = "Chicken breast (grilled, skinless, 100g)", Calories = 165 },
                new Food { FoodId = 6, Name = "Salmon (cooked, 100g)", Calories = 206 },
                new Food { FoodId = 7, Name = "Egg (large, boiled)", Calories = 78 },
                new Food { FoodId = 8, Name = "Almonds (raw, 28g)", Calories = 164 },
                new Food { FoodId = 9, Name = "Avocado (medium)", Calories = 322 },
                new Food { FoodId = 10, Name = "Broccoli (raw, 1 cup)", Calories = 31 },
                new Food { FoodId = 11, Name = "Carrot (medium)", Calories = 25 },
                new Food { FoodId = 12, Name = "Cheddar cheese (28g)", Calories = 113 },
                new Food { FoodId = 13, Name = "Whole milk (1 cup)", Calories = 149 },
                new Food { FoodId = 14, Name = "Greek yogurt (plain, low-fat, 100g)", Calories = 73 },
                new Food { FoodId = 15, Name = "Potato (baked, medium)", Calories = 161 },
                new Food { FoodId = 16, Name = "Beef steak (grilled, lean, 100g)", Calories = 271 },
                new Food { FoodId = 17, Name = "Pasta (cooked spaghetti, 1 cup)", Calories = 221 },
                new Food { FoodId = 18, Name = "Oatmeal (cooked, 1 cup)", Calories = 166 },
                new Food { FoodId = 19, Name = "Orange (medium)", Calories = 62 },
                new Food { FoodId = 20, Name = "Strawberries (1 cup)", Calories = 46 }
            );

            // 可选：给 Calorie 设置默认值
            modelBuilder.Entity<Calorie>()
                .Property(c => c.DistanceTaken)
                .HasDefaultValue(0);
        }
    }
}
