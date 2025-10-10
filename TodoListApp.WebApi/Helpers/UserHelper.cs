using System.Globalization;
using System.Security.Claims;

namespace TodoListApp.WebApi.Helpers;

/// <summary>
/// Helper class for user-related operations.
/// </summary>
internal static class UserHelper
{
    /// <summary>
    /// Gets the current user's ID from the claims principal.
    /// </summary>
    /// <param name="user">The claims principal representing the current user.</param>
    /// <returns>The user ID if available; otherwise, null.</returns>
    public static int? GetCurrentUserId(ClaimsPrincipal user)
    {
        if (user == null)
        {
            return null;
        }

        var userNameIdentifier = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userNameIdentifier != null &&
            int.TryParse(userNameIdentifier, NumberStyles.Integer, CultureInfo.InvariantCulture, out var userId))
        {
            return userId;
        }

        return null;
    }
}
