using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CalorieBurnMgt.DTOs
{
    public class ForgotPasswordRequestDto
    {
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Display(Name = "Email")]  // DTO is used, keep Display for UI label
        public string Email { get; set; }
    }
}
