namespace CalorieBurnMgt.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Weight { get; set; } // in kilograms
        public int Height { get; set; } // in centimeters

        public int BMI         {
            get
            {
                double heightInMeters = Height / 100.0;
                return (int)(Weight / (heightInMeters * heightInMeters));
            }
        }

        public Calorie Calorie  { get; set; }
    }
}
