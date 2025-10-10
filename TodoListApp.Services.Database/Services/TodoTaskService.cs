using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Database.Entities;
using TodoListApp.Services.Database.Helpers;
using TodoListApp.Services.Database.Repositories;
using TodoListApp.Services.Database.ServiceExeptions;
using TodoListApp.Services.Enums;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;

namespace TodoListApp.Services.Database.Services;

/// <summary>
/// Service for managing to-do tasks.
/// </summary>
public class TodoTaskService : ITodoTaskService
{
    private readonly TodoTaskRepository repository;
    private readonly CommentRepository commentRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoTaskService"/> class.
    /// </summary>
    /// <param name="context">The to-do lists database context.</param>
    public TodoTaskService(TodoListDbContext context)
    {
        this.repository = new TodoTaskRepository(context);
        this.commentRepository = new CommentRepository(context);
    }

    /// <summary>
    /// Adds a new to-do task.
    /// </summary>
    /// <param name="model">The model containing the info about the new task.</param>
    /// <returns>The added to-do task model.</returns>
    /// <exception cref="ArgumentNullException">If the model is null.</exception>
    /// <exception cref="EntityWithIdExistsException">If a task with the same ID already exists.</exception>
    public async Task<TodoTaskModel> AddAsync(TodoTaskModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        TodoTask? existing = await this.repository.GetByIdAsync(model.Id);
        if (existing is not null)
        {
            throw new EntityWithIdExistsException(nameof(TodoTask), model.Id);
        }

        try
        {
            model.CreationDate = null;
            TodoTask newTask = await this.repository.AddAsync(ModelToEntityConverter.ToTaskEntity(model));
            return EntityToModelConverter.ToTodoTaskModel(newTask);
        }
        catch (DbUpdateException ex)
        {
            throw new UnableToCreateException(nameof(TodoList), ex, model.Id);
        }
    }

    /// <summary>
    /// Deletes a to-do task by its ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="id">The unique identifier of the task.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    /// <exception cref="EntityNotFoundException">If there is no entity with the specified ID in the context.</exception>
    /// <exception cref="UnauthorizedAccessException">If the user has no permission to delete the task.</exception>
    /// <exception cref="UnableToDeleteException">Is service is unable to delete the task.</exception>
    public async Task DeleteAsync(int userId, int id)
    {
        TodoTask? existing = await this.repository.GetByIdAsync(id);
        if (existing is null)
        {
            throw new EntityNotFoundException(nameof(existing), id);
        }
        else if (existing.TodoList.OwnerId != userId &&
            !existing.TodoList.TodoListUserRoles.Any(lur => lur.UserId == userId && lur.ListRole.RoleName == "Editor"))
        {
            throw new UnauthorizedAccessException("You do not have permission to delete this task.");
        }

        try
        {
            bool success = await this.repository.DeleteByIdAsync(id);
            if (!success)
            {
                throw new UnableToDeleteException(nameof(existing), id);
            }
        }
        catch (DbUpdateException ex)
        {
            throw new UnableToDeleteException(nameof(existing), id, ex);
        }
    }

    /// <summary>
    /// Gets all to-do tasks the specified user hass access to.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A read-only list of to-do task models.</returns>
    public async Task<IReadOnlyList<TodoTaskModel>> GetAllAsync(int userId)
    {
        var lists = await this.repository.GetAllUserTasksAsync(userId);
        return lists
            .Select(l =>
            EntityToModelConverter.ToTodoTaskModel(l))
            .ToList();
    }

    /// <summary>
    /// Gets all to-do tasks the specified user has access to with pagination.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of tasks on the page.</param>
    /// <returns>A read-only list of to-do task models.</returns>
    public async Task<IReadOnlyList<TodoTaskModel>> GetAllAsync(int userId, int pageNumber, int rowCount)
    {
        var lists = await this.repository.GetAllUserTasksAsync(userId, pageNumber, rowCount);
        return lists
            .Select(l =>
            EntityToModelConverter.ToTodoTaskModel(l))
            .ToList();
    }

    /// <summary>
    /// Gets a to-do task by its ID if the specified user has access to it.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="id">The unique identifier of the task.</param>
    /// <returns>The to-do task model.</returns>
    /// <exception cref="EntityNotFoundException">If there is no entity with the specified ID in the context.</exception>
    /// <exception cref="UnauthorizedAccessException">If the user has no rights to see the task.</exception>
    public async Task<TodoTaskModel> GetByIdAsync(int userId, int id)
    {
        var entity = await this.repository.GetByIdAsync(id);

        if (entity is null)
        {
            throw new EntityNotFoundException(nameof(entity), id);
        }
        else if (entity.TodoList.OwnerId != userId &&
            !entity.TodoList.TodoListUserRoles.Any(lur => lur.UserId == userId))
        {
            throw new UnauthorizedAccessException("You do not have permission to see this task.");
        }

        return EntityToModelConverter.ToTodoTaskModel(entity);
    }

    /// <summary>
    /// Updates an existing to-do task.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="model">The model containing the new info about the task.</param>
    /// <returns>The updated to-do task model.</returns>
    /// <exception cref="EntityNotFoundException">If there is no task with the specified ID in the context.</exception>
    /// <exception cref="UnauthorizedAccessException">If the user has no rights to edit the task.</exception>
    /// <exception cref="UnableToUpdateException">If the service is unable to update the task.</exception>
    public async Task<TodoTaskModel> UpdateAsync(int userId, TodoTaskModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        TodoTask? existing = await this.repository.GetByIdAsync(model.Id);
        if (existing is null)
        {
            throw new EntityNotFoundException(nameof(existing), model.Id);
        }
        else if (existing.TodoList.OwnerId != userId &&
            !existing.TodoList.TodoListUserRoles.Any(lur => lur.UserId == userId && lur.ListRole.RoleName == "Editor"))
        {
            throw new UnauthorizedAccessException("You do not have permission to update this task.");
        }

        try
        {
            model.CreationDate = existing.CreationDate;
            TodoTask? updated = await this.repository.UpdateAsync(ModelToEntityConverter.ToTaskEntity(model));
            return updated is null ?
                throw new UnableToUpdateException(nameof(existing), model.Id) :
                EntityToModelConverter.ToTodoTaskModel(updated);
        }
        catch (DbUpdateException ex)
        {
            throw new UnableToUpdateException(nameof(existing), model.Id, ex);
        }
    }

    /// <summary>
    /// Gets all to-do tasks by the list ID with filtering, sorting, and pagination.
    /// </summary>
    /// <param name="id">The unqiue identifier of the list.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="filter">The filtering by task status.</param>
    /// <param name="sortBy">The sorting property.</param>
    /// <param name="sortOrder">The sortyng direction.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of tasks on the page.</param>
    /// <returns>A read-only list of to-do task models.</returns>
    public async Task<IReadOnlyList<TodoTaskModel>> GetAllByListIdAsync(int id, int userId, TaskFilter filter = TaskFilter.Active, string? sortBy = "DueDate", string? sortOrder = "asc", int? pageNumber = null, int? rowCount = null)
    {
        sortOrder = string.Equals(sortOrder, "asc", StringComparison.OrdinalIgnoreCase) ? sortOrder : "desc";
        sortBy = string.Equals(sortOrder, "Title", StringComparison.OrdinalIgnoreCase) ? sortBy : "DueDate";

        var tasks = await this.repository.GetAllByListIdAsync(id);

        var filteredTasks = filter switch
        {
            TaskFilter.InProgress => tasks.Where(t => t.StatusId == 2),
            TaskFilter.NotStarted => tasks.Where(t => t.StatusId == 1),
            TaskFilter.Completed => tasks.Where(t => t.StatusId == 3),
            TaskFilter.Active => tasks.Where(t => t.StatusId != 3),
            TaskFilter.All => tasks,
            _ => tasks
        };

        if (pageNumber != null && rowCount != null)
        {
            int page = pageNumber > 0 ? (int)pageNumber : 1;
            int row = rowCount > 0 ? (int)rowCount : 1;

            filteredTasks = filteredTasks
                .Skip((page - 1) * row)
                .Take(row)
                .ToList();
        }

        var sortedTasks = Sorters.ApplyTasksSorting(filteredTasks.AsQueryable(), sortBy!, sortOrder!);
        return sortedTasks.Select(t => EntityToModelConverter.ToTodoTaskModel(t)).ToList();
    }

    /// <summary>
    /// Gets the count of to-do tasks by the list ID with filtering.
    /// </summary>
    /// <param name="id">The unique identifier of the list.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="filter">The task status filter.</param>
    /// <returns>The count of to-do tasks.</returns>
    public async Task<int> GetAllByListIdCountAsync(int id, int userId, TaskFilter filter = TaskFilter.Active)
    {
        var tasks = await this.repository.GetAllByListIdAsync(id);

        int count = filter switch
        {
            TaskFilter.InProgress => tasks.Count(t => t.StatusId == 2),
            TaskFilter.NotStarted => tasks.Count(t => t.StatusId == 1),
            TaskFilter.Completed => tasks.Count(t => t.StatusId == 3),
            TaskFilter.Active => tasks.Count(t => t.StatusId != 3),
            TaskFilter.All => tasks.Count,
            _ => tasks.Count
        };

        return count;
    }

    /// <summary>
    /// Updates the status of a to-do task.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="taskId">The unique identifier of the task.</param>
    /// <param name="statusId">The unique identifier of the status.</param>
    /// <returns>The updated to-do task model.</returns>
    /// <exception cref="EntityNotFoundException">If there is not task with the specifed ID in the context.</exception>
    /// <exception cref="UnauthorizedAccessException">If the use has no rights to edit the task.</exception>
    /// <exception cref="UnableToUpdateException">If the service is unable to update the task.</exception>
    public async Task<TodoTaskModel> UpdateTaskStatusAsync(int userId, int taskId, int statusId)
    {
        TodoTask? existing = await this.repository.GetByIdAsync(taskId);
        if (existing is null)
        {
            throw new EntityNotFoundException(nameof(existing), taskId);
        }
        else if (existing.TodoList.OwnerId != userId &&
            !existing.TodoList.TodoListUserRoles.Any(lur => lur.UserId == userId && lur.ListRole.RoleName != "Editor"))
        {
            throw new UnauthorizedAccessException("You do not have permission to update this task.");
        }

        try
        {
            existing.StatusId = statusId;
            var model = EntityToModelConverter.ToTodoTaskModel(existing);
            TodoTask? updated = await this.repository.UpdateAsync(ModelToEntityConverter.ToTaskEntity(model));
            return updated is null ?
                throw new UnableToUpdateException(nameof(existing), model.Id) :
                EntityToModelConverter.ToTodoTaskModel(updated);
        }
        catch (DbUpdateException ex)
        {
            throw new UnableToUpdateException(nameof(existing), taskId, ex);
        }
    }

    /// <summary>
    /// Retrieves the total number of comments associated with a specific task.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="userId">The ID of the user requesting the count.</param>
    /// <returns>The total number of comments for the given task.</returns>
    public async Task<int> GetTaskCommentsCountAsync(int taskId, int userId)
    {
        return (await this.GetByIdAsync(userId, taskId)).UserComments.Count;
    }

    /// <summary>
    /// Retrieves all comments associated with a specific task, optionally paginated.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="userId">The ID of the user requesting the comments.</param>
    /// <param name="pageNumber">The optional page number for pagination.</param>
    /// <param name="rowCount">The optional number of comments per page.</param>
    /// <returns>A read-only list of <see cref="CommentModel"/> representing the comments.</returns>
    public async Task<IReadOnlyList<CommentModel>> GetTaskCommentsAsync(int taskId, int userId, int? pageNumber = null, int? rowCount = null)
    {
        var commetns = (await this.GetByIdAsync(userId, taskId))
            .UserComments;

        if (pageNumber != null && rowCount != null)
        {
            int page = pageNumber > 0 ? (int)pageNumber : 1;
            int row = rowCount > 0 ? (int)rowCount : 1;
            return commetns
                .Skip((page - 1) * row)
                .Take(row)
                .ToList();
        }

        return commetns.ToList();
    }

    /// <summary>
    /// Retrieves a specific comment from a task by its identifier.
    /// </summary>
    /// <param name="userId">The ID of the user requesting the comment.</param>
    /// <param name="taskId">The ID of the task that contains the comment.</param>
    /// <param name="commentId">The ID of the comment to retrieve.</param>
    /// <returns>The <see cref="CommentModel"/> representing the requested comment.</returns>
    /// <exception cref="EntityNotFoundException">Thrown when the specified comment does not exist.</exception>
    public async Task<CommentModel> GetCommentByIdAsync(int userId, int taskId, int commentId)
    {
        TodoTaskModel existing = await this.GetByIdAsync(userId, taskId);

        var comment = existing.UserComments
            .FirstOrDefault(c => c.Id == commentId);

        return comment is null ?
            throw new EntityNotFoundException(nameof(comment), commentId) :
            comment;
    }

    /// <summary>
    /// Adds a new comment to a task.
    /// </summary>
    /// <param name="commentModel">The comment model to add.</param>
    /// <returns>The created <see cref="CommentModel"/> instance.</returns>
    /// <exception cref="EntityNotFoundException">Thrown when the specified task does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to add a comment.</exception>
    /// <exception cref="UnableToCreateException">Thrown when the comment could not be created.</exception>
    public async Task<CommentModel> AddTaskCommentAsync(CommentModel commentModel)
    {
        ArgumentNullException.ThrowIfNull(commentModel);

        TodoTask? existing = await this.repository.GetByIdAsync(commentModel.TaskId);
        if (existing is null)
        {
            throw new EntityNotFoundException(nameof(existing), commentModel.TaskId);
        }
        else if (existing.TodoList.OwnerId != commentModel.UserId &&
            !existing.TodoList.TodoListUserRoles.Any(lur => lur.UserId == commentModel.UserId && lur.ListRole.RoleName == "Editor"))
        {
            throw new UnauthorizedAccessException("You do not have permission to update this task.");
        }

        try
        {
            Comment? created = await this.commentRepository.AddAsync(ModelToEntityConverter.ToCommentEntity(commentModel));
            return created is null ?
                throw new UnableToCreateException(nameof(Comment)) :
                EntityToModelConverter.ToCommentModel(created);
        }
        catch (DbUpdateException ex)
        {
            throw new UnableToCreateException(nameof(Comment), ex, commentModel.Id);
        }
    }

    /// <summary>
    /// Updates an existing comment on a task.
    /// </summary>
    /// <param name="userId">The ID of the user performing the update.</param>
    /// <param name="commentModel">The comment model containing updated data.</param>
    /// <returns>The updated <see cref="CommentModel"/> instance.</returns>
    /// <exception cref="EntityNotFoundException">Thrown when the comment or task does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to update the comment.</exception>
    /// <exception cref="UnableToUpdateException">Thrown when the update fails.</exception>
    public async Task<CommentModel> UpdateTaskCommentAsync(int userId, CommentModel commentModel)
    {
        ArgumentNullException.ThrowIfNull(commentModel);
        Comment? existingComment = await this.commentRepository.GetByIdAsync(commentModel.Id);
        if (existingComment is null)
        {
            throw new EntityNotFoundException(nameof(existingComment), commentModel.Id);
        }

        TodoTask? existing = await this.repository.GetByIdAsync(existingComment.TaskId);
        if (existing is null)
        {
            throw new EntityNotFoundException(nameof(existing), commentModel.TaskId);
        }
        else if (existing.TodoList.OwnerId != userId && userId != existingComment.UserId)
        {
            throw new UnauthorizedAccessException("You do not have permission to update this comment.");
        }

        try
        {
            Comment? updated = await this.commentRepository.UpdateAsync(ModelToEntityConverter.ToCommentEntity(commentModel));
            return updated is null ?
                throw new UnableToUpdateException(nameof(commentModel), commentModel.Id) :
                EntityToModelConverter.ToCommentModel(updated);
        }
        catch (DbUpdateException ex)
        {
            throw new UnableToUpdateException(nameof(existing), commentModel.Id, ex);
        }
    }

    /// <summary>
    /// Removes a specific comment from a task.
    /// </summary>
    /// <param name="userId">The ID of the user performing the removal.</param>
    /// <param name="taskId">The ID of the task containing the comment.</param>
    /// <param name="commentId">The ID of the comment to remove.</param>
    /// <exception cref="EntityNotFoundException">Thrown when the comment or task does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to delete the comment.</exception>
    /// <exception cref="UnableToDeleteException">Thrown when the deletion fails.</exception>
    public async Task RemoveTaskCommentAsync(int userId, int taskId, int commentId)
    {
        Comment? existingComment = await this.commentRepository.GetByIdAsync(commentId);
        if (existingComment is null)
        {
            throw new EntityNotFoundException(nameof(existingComment), commentId);
        }

        TodoTask? existing = await this.repository.GetByIdAsync(taskId);
        if (existing is null)
        {
            throw new EntityNotFoundException(nameof(existing), taskId);
        }
        else if (existing.TodoList.OwnerId != userId && userId != existingComment.UserId)
        {
            throw new UnauthorizedAccessException("You do not have permission to update this comment.");
        }

        try
        {
            var success = await this.commentRepository.DeleteByIdAsync(commentId);
            if (!success)
            {
                throw new UnableToDeleteException(nameof(existingComment), commentId);
            }
        }
        catch (DbUpdateException ex)
        {
            throw new UnableToDeleteException(nameof(existing), commentId, ex);
        }
    }
}
