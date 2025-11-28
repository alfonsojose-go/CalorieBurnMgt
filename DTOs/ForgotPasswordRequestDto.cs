using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CalorieBurnMgt.DTOs
{
    public class ForgotPasswordRequestDto
    {
        

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email format is invalid")]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
    }
}
