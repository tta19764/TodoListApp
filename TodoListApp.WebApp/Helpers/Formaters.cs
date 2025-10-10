using TodoListApp.Services.Enums;

namespace TodoListApp.WebApp.Helpers;

/// <summary>
/// The static class containig various format methods for to-do list app.
/// </summary>
public static class Formaters
{
    /// <summary>
    /// Formats the owner's name as "FirstName L." or "N/A" if both names are missing.
    /// </summary>
    /// <param name="firstName">The user first name.</param>
    /// <param name="lastName">The user last name.</param>
    /// <returns>The formatted name string.</returns>
    public static string FormatOwnerName(string? firstName, string? lastName)
    {
        if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
        {
            return "N/A";
        }

        var name = firstName ?? "N/A";
        if (!string.IsNullOrEmpty(lastName))
        {
            name += $" {lastName[0]}.";
        }

        return name;
    }

    /// <summary>
    /// Converts a string representation of a role to the corresponding ListRole enum value.
    /// </summary>
    /// <param name="roleName">The role name as a string.</param>
    /// <returns>The corresponding ListRole enum value.</returns>
    public static ListRole StringToRoleEnum(string? roleName)
    {
        return roleName?.ToUpperInvariant() switch
        {
            "OWNER" => ListRole.Owner,
            "EDITOR" => ListRole.Editor,
            "VIEWER" => ListRole.Viewer,
            _ => ListRole.None,
        };
    }

    /// <summary>
    /// Converts a string representation of a task filter to the corresponding TaskFilter enum value.
    /// </summary>
    /// <param name="filter">The filter name as a string.</param>
    /// <returns>The corresponding TaskFilter enum value.</returns>
    public static TaskFilter StringToTaskFilterEnum(string? filter)
    {
        return filter?.ToUpperInvariant() switch
        {
            "NOTSTARTED" => TaskFilter.NotStarted,
            "INPROGRESS" => TaskFilter.InProgress,
            "COMPLETED" => TaskFilter.Completed,
            "ALL" => TaskFilter.All,
            _ => TaskFilter.Active
        };
    }
}
