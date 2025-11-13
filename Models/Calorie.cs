namespace CalorieBurnMgt.Models
{
    public class Calorie
    {
        public int CalorieId { get; set; }
        public int UserId { get; set; }
        public int FoodId { get; set; }
        public DateTime DateConsumed { get; set; }

        public DateTime DateBurned { get; set; }
        public int CaloriesTaken { get; set; } // in kcal

        public int CaloriesBurned { get; set; } // in kcal

        public int TotalCalorieBalance
        {
            get
            {
                return CaloriesTaken - CaloriesBurned;
            }
        }
    }
}
