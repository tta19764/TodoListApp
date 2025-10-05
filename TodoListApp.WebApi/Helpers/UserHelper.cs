using System.Globalization;
using System.Security.Claims;

namespace TodoListApp.WebApi.Helpers;

internal static class UserHelper
{
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
