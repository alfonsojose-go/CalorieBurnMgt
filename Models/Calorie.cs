namespace CalorieBurnMgt.Models
{
    public class Calorie
    {
        public int CalorieId { get; set; }

        // 改为 string 类型，匹配 IdentityUser.Id
        public string UserId { get; set; }

        public int FoodId { get; set; } 

        // 导航属性
        public User User { get; set; }      // EF Core 会自动识别一对多关系
        public Food Food { get; set; } 

        public DateTime DateConsumed { get; set; }

        public DateTime DateBurned { get; set; }

        public int CaloriesTaken { get; set; } // in kcal

        public int DistanceTaken { get; set; } // in kilometers

        public int CaloriesBurned { get; set; } // = to formula of David
    }
}
