
namespace CalorieBurnMgt.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; } = string.Empty; // IdentityUser.Id is string
        public string FullName { get; set; } = string.Empty; // according User.FullName 
        public double BMI { get; set; }
        public double TotalCalorieBalance { get; set; }
        public int Age { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
    }
}
