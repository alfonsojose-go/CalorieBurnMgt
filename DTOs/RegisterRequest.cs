using System.ComponentModel.DataAnnotations;

namespace CalorieBurnMgt.DTOs
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty; // Added on 2025-11-23

        [Required]
        public string UserName { get; set; } = string.Empty; // Username for login

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Range(0, 150)]
        public int Age { get; set; }

        [Range(0, 500)]
        public int Weight { get; set; } // Unit: kg

        [Range(0, 300)]
        public int Height { get; set; } // Unit: cm
    }
}
