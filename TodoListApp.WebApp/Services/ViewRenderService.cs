using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using TodoListApp.Services.Interfaces.Servicies;

namespace TodoListApp.WebApp.Services;

/// <summary>
/// Service for rendering Razor views to HTML strings for email templates.
/// </summary>
public class ViewRenderService : IViewRenderService
{
    private readonly IRazorViewEngine razorViewEngine;
    private readonly ITempDataProvider tempDataProvider;
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewRenderService"/> class.
    /// </summary>
    /// <param name="razorViewEngine">The view engine.</param>
    /// <param name="tempDataProvider">The template provider.</param>
    /// <param name="serviceProvider">The service provider.</param>
    /// <exeption cref="ArgumentNullException">Thrown when any parameter is null.</exception>
    public ViewRenderService(
        IRazorViewEngine razorViewEngine,
        ITempDataProvider tempDataProvider,
        IServiceProvider serviceProvider)
    {
        this.razorViewEngine = razorViewEngine ?? throw new ArgumentNullException(nameof(razorViewEngine));
        this.tempDataProvider = tempDataProvider ?? throw new ArgumentNullException(nameof(tempDataProvider));
        this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    /// <summary>
    /// Renders a Razor view to an HTML string.
    /// </summary>
    /// <typeparam name="TModel">The view model.</typeparam>
    /// <param name="viewName">The view model name.</param>
    /// <param name="model">The model.</param>
    /// <returns>The rendered HTML string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when viewName is null.</exception>
    public async Task<string> RenderToStringAsync<TModel>(string viewName, TModel model)
    {
        var httpContext = new DefaultHttpContext { RequestServices = this.serviceProvider };
        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

        using var sw = new StringWriter();

        var viewResult = this.razorViewEngine.FindView(actionContext, viewName, false);

        if (viewResult.View == null)
        {
            throw new ArgumentNullException($"Unable to find view '{viewName}'");
        }

        var viewDictionary = new ViewDataDictionary<TModel>(
            new EmptyModelMetadataProvider(),
            new ModelStateDictionary())
        {
            Model = model,
        };

        var viewContext = new ViewContext(
            actionContext,
            viewResult.View,
            viewDictionary,
            new TempDataDictionary(actionContext.HttpContext, this.tempDataProvider),
            sw,
            new HtmlHelperOptions());

        await viewResult.View.RenderAsync(viewContext);
        return sw.ToString();
    }
}
