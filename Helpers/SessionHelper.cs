using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CalorieBurnMgt.Helpers
{
    public class SessionHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Get user ID as string (for Identity's GUID/string IDs)
        public string GetCurrentUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user?.Identity?.IsAuthenticated != true)
            {
                throw new UnauthorizedAccessException("User must be logged in");
            }

            // Method 1: Try to get user ID from claims
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Method 2: If not found in claims, get from User.Identity.Name
            if (string.IsNullOrEmpty(userId))
            {
                userId = user.Identity.Name;

                // If it's a username, we need to look up the actual user ID
                if (!string.IsNullOrEmpty(userId))
                {
                    // You might need to inject UserManager for this
                    throw new UnauthorizedAccessException("Username found but need to look up User ID. Please ensure claims are properly set.");
                }
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User ID not found. Please log in again.");
            }

            return userId;
        }

        // Check if user is logged in
        public bool IsUserLoggedIn()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;
        }

        // Get user ID without throwing exception
        public string? TryGetCurrentUserId()
        {
            try
            {
                return GetCurrentUserId();
            }
            catch
            {
                return null;
            }
        }
    }
}