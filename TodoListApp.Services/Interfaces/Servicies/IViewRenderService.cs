namespace TodoListApp.Services.Interfaces.Servicies;

/// <summary>
/// Service for rendering Razor views to HTML strings.
/// </summary>
public interface IViewRenderService
{
    /// <summary>
    /// Renders a Razor view to an HTML string.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <param name="viewName">The name of the view (e.g., "Emails/PasswordReset").</param>
    /// <param name="model">The model to pass to the view.</param>
    /// <returns>The rendered HTML as a string.</returns>
    Task<string> RenderToStringAsync<TModel>(string viewName, TModel model);
}
