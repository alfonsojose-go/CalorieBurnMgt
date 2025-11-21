using Microsoft.AspNetCore.Http;
using System;

namespace CalorieBurnMgt.Helpers
{
    public class SessionHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Get the current user ID from session
        public int GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.Session.GetInt32("UserId");

            if (userId == null || userId == 0)
            {
                throw new UnauthorizedAccessException("User must be logged in");
            }

            return userId.Value;
        }

        // Set user ID in session (use this in your Login action)
        public void SetUserId(int userId)
        {
            _httpContextAccessor.HttpContext?.Session.SetInt32("UserId", userId);
        }

        // Check if user is logged in
        public bool IsUserLoggedIn()
        {
            var userId = _httpContextAccessor.HttpContext?.Session.GetInt32("UserId");
            return userId != null && userId > 0;
        }

        // Clear user session (use this in your Logout action)
        public void ClearSession()
        {
            _httpContextAccessor.HttpContext?.Session.Clear();
        }

        // Get user ID without throwing exception (returns null if not logged in)
        public int? TryGetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.Session.GetInt32("UserId");
        }
    }
}