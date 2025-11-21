namespace CalorieBurnMgt.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Weight { get; set; } // in kilograms
        public int Height { get; set; } // in centimeters

        public int BMI
        {
            get
            {
                double heightInMeters = Height / 100.0;
                return (int)(Weight / (heightInMeters * heightInMeters));
            }
        }

        // Navigation property - changed to collection
        public ICollection<Calorie> Calories { get; set; }

        // Calculate total calorie balance
        public int TotalCalorieBalance
        {
            get
            {
                if (Calories == null || !Calories.Any())
                    return 0;

                int totalTaken = Calories.Sum(c => c.CaloriesTaken);
                int totalBurned = Calories.Sum(c => c.CaloriesBurned);
                return totalTaken - totalBurned;
            }
        }
    }
}