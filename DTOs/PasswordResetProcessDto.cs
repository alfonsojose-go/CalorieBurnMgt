namespace CalorieBurnMgt.DTOs
{
    public class PasswordResetProcessDto
    {
        public string Email { get; set; }           // Email entered by the user
        public string UserId { get; set; }          // User ID retrieved from database (populated by service layer)
        public string Token { get; set; }           // Generated reset token (populated by service layer)
        public string ClientIp { get; set; }        // Client IP (for logging and security)
        public string UserAgent { get; set; }       // Browser information (for logging)
        public DateTime RequestedAt { get; set; }   // Request time (used for expiration check)
    }
}