using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CalorieBurnMgt.DTOs
{
    public class LoginRequest
    {
        [Required]
        [JsonPropertyName("username")]   // 🔥 Must include this line!
        public string UserName { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("password")]   // 🔥 Must include this line!
        public string Password { get; set; } = string.Empty;
    }
}
