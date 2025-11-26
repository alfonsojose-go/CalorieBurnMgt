using Microsoft.AspNetCore.Identity;
namespace CalorieBurnMgt.Models
{
    public class User : IdentityUser
    {
        // Personal information
        public string FullName { get; set; } = string.Empty; // More formal than Name
        public int Age { get; set; }
        public int Weight { get; set; } // Unit specified
        public int Height { get; set; } // Unit specified

        // Membership information
        public bool IsPremium { get; set; } = false; // Is paid member
        public DateTime? PremiumExpireDate { get; set; } // Membership expiration date

        // Identity handles password management; no need for PasswordHash / ResetToken
        // PasswordHash is provided by IdentityUser
        // ResetToken and ResetTokenExpiration are managed internally by Identity
        public double BMI
        {
            get
            {
                if (Height <= 0) return 0; // Prevent division by zero
                double heightInMeters = Height / 100.0;
                double bmi = Weight / (heightInMeters * heightInMeters);
                return Math.Round(bmi, 2); // Keep two decimal places
            }
        }

        // Navigation property - changed to collection
        public ICollection<Calorie> Calories { get; set; } = new List<Calorie>();

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
