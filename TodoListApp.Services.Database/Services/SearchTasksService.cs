using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Database.Helpers;
using TodoListApp.Services.Database.Repositories;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;

namespace TodoListApp.Services.Database.Services;
public class SearchTasksService : ISearchTasksService
{
    private readonly TodoTaskRepository repository;

    public SearchTasksService(TodoListDbContext context)
    {
        this.repository = new TodoTaskRepository(context);
    }

    /// <summary>
    /// Searches for to-do tasks based on optional criteria: title, creation date, and due date with pagination.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="title">Task title.</param>
    /// <param name="creationDate">Task creation date.</param>
    /// <param name="dueDate">Task due date.</param>
    /// <param name="pageNumber">Page number.</param>
    /// <param name="rowCount">The number of models on the page.</param>
    /// <returns>A read-only list of to-do task models.</returns>
    public async Task<IReadOnlyList<TodoTaskModel>> SearchTasksAsync(int userId, string? title = null, DateTime? creationDate = null, DateTime? dueDate = null, int? pageNumber = null, int? rowCount = null)
    {
        if (pageNumber != null && rowCount != null)
        {
            int page = pageNumber > 0 ? (int)pageNumber : 1;
            int row = rowCount > 0 ? (int)rowCount : 1;

            return (await this.repository.SerchTasksAsync(userId, title, creationDate, dueDate, page, row)).Select(t => EntityToModelConverter.ToTodoTaskModel(t)).ToList();
        }
        else
        {
            return (await this.repository.SerchTasksAsync(userId, title, creationDate, dueDate)).Select(t => EntityToModelConverter.ToTodoTaskModel(t)).ToList();
        }
    }

    /// <summary>
    /// Gets the count of to-do tasks based on optional criteria: title, creation date, and due date.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="title">The task title.</param>
    /// <param name="creationDate">The task creation date.</param>
    /// <param name="dueDate">The task due date.</param>
    /// <returns>The count of to-do tasks.</returns>
    public async Task<int> GetAllSearchCountAsync(int userId, string? title = null, DateTime? creationDate = null, DateTime? dueDate = null)
    {
        return (await this.repository.SerchTasksAsync(userId, title, creationDate, dueDate)).Count;
    }
}
