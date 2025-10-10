using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Database.ServiceExeptions;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;
using TodoListApp.WebApi.CustomLogs;
using TodoListApp.WebApi.Helpers;
using TodoListApp.WebApi.Models.Dtos.Create;
using TodoListApp.WebApi.Models.Dtos.Read;
using TodoListApp.WebApi.Models.Dtos.Update;

namespace TodoListApp.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TagsController : ControllerBase
{
    private readonly ITagService service;
    private readonly ILogger<TagsController> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TagsController"/> class.
    /// </summary>
    /// <param name="service">The service for managing tags.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">Thrown when a required parameter is null.</exception>
    public TagsController(
        ITagService service,
        ILogger<TagsController> logger)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets all tags.
    /// </summary>
    /// <returns>A list of all tags.</returns>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<TagDto>>> GetAllTags()
    {
        try
        {
            var tags = await this.service.GetAllAsync();

            if (tags == null || !tags.Any())
            {
                return this.Ok(new List<TagDto>());
            }

            var tagDtos = tags.Select(MapToDto.ToTagDto).ToList();

            TagLog.LogTagsRetrieved(this.logger, tagDtos.Count);

            return this.Ok(tagDtos);
        }
        catch (Exception ex)
        {
            TagLog.LogUnexpectedErrorRetrievingTags(this.logger, ex);
            return this.StatusCode(500, "An unexpected error occurred while retrieving tags.");
            throw;
        }
    }

    /// <summary>
    /// Gets paginated tags.
    /// </summary>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of tags per page.</param>
    /// <returns>A paginated list of tags.</returns>
    [HttpGet("{pageNumber:min(1)}/{rowCount:min(1)}")]
    [Authorize]
    public async Task<ActionResult<List<TagDto>>> GetAllTagsPaginated(
        int pageNumber,
        int rowCount)
    {
        try
        {
            var tags = await this.service.GetAllAsync(pageNumber, rowCount);

            if (tags == null || !tags.Any())
            {
                return this.Ok(new List<TagDto>());
            }

            var tagDtos = tags.Select(MapToDto.ToTagDto).ToList();

            TagLog.LogTagsRetrieved(this.logger, tagDtos.Count);

            return this.Ok(tagDtos);
        }
        catch (ArgumentOutOfRangeException ex) when (ex.ParamName == "pageNumber" || ex.ParamName == "rowCount")
        {
            TagLog.LogInvalidPaginationParameters(this.logger, pageNumber, rowCount);
            return this.BadRequest("Invalid pagination parameters. Page number and row count must be positive integers.");
        }
        catch (Exception ex)
        {
            TagLog.LogUnexpectedErrorRetrievingTags(this.logger, ex);
            return this.StatusCode(500, "An unexpected error occurred while retrieving tags.");
            throw;
        }
    }

    /// <summary>
    /// Gets a specific tag by its ID.
    /// </summary>
    /// <param name="tagId">The unique identifier of the tag.</param>
    /// <returns>The tag with the specified ID.</returns>
    [HttpGet("{tagId:int}")]
    [Authorize]
    public async Task<ActionResult<TagDto>> GetTag(int tagId)
    {
        try
        {
            var tag = await this.service.GetByIdAsync(tagId);

            if (tag == null)
            {
                TagLog.LogTagNotFound(this.logger, tagId);
                return this.NotFound($"Tag with ID {tagId} was not found.");
            }

            TagLog.LogTagRetrieved(this.logger, tagId);

            var tagDto = MapToDto.ToTagDto(tag);
            return this.Ok(tagDto);
        }
        catch (EntityNotFoundException)
        {
            TagLog.LogTagNotFound(this.logger, tagId);
            return this.NotFound($"Tag with ID {tagId} was not found.");
        }
        catch (Exception ex)
        {
            TagLog.LogUnexpectedErrorRetrievingTag(this.logger, tagId, ex);
            return this.StatusCode(500, "An unexpected error occurred while retrieving the tag.");
            throw;
        }
    }

    /// <summary>
    /// Gets the count of available tags for a specific task.
    /// </summary>
    /// <param name="taskId">The unique identifier of the task.</param>
    /// <returns>The count of available tags.</returns>
    [HttpGet("Available/{taskId:int}/Count")]
    [Authorize]
    public async Task<ActionResult<int>> GetAvailableTagsCount(int taskId)
    {
        try
        {
            var count = await this.service.GetAvilableTaskTagsCountAsync(taskId);

            return this.Ok(count);
        }
        catch (Exception ex)
        {
            TagLog.LogUnexpectedErrorRetrievingAvailableTags(this.logger, taskId, ex);
            return this.StatusCode(500, "An unexpected error occurred while retrieving available tags count.");
            throw;
        }
    }

    /// <summary>
    /// Gets all available tags for a specific task (tags not yet assigned to the task).
    /// </summary>
    /// <param name="taskId">The unique identifier of the task.</param>
    /// <returns>A list of available tags.</returns>
    [HttpGet("Available/{taskId:int}")]
    [Authorize]
    public async Task<ActionResult<List<TagDto>>> GetAvailableTags(int taskId)
    {
        try
        {
            var tags = await this.service.GetAvailableTaskTagsAsync(taskId);

            if (tags == null || !tags.Any())
            {
                return this.Ok(new List<TagDto>());
            }

            var tagDtos = tags.Select(MapToDto.ToTagDto).ToList();

            TagLog.LogAvailableTagsRetrieved(this.logger, tagDtos.Count, taskId);

            return this.Ok(tagDtos);
        }
        catch (Exception ex)
        {
            TagLog.LogUnexpectedErrorRetrievingAvailableTags(this.logger, taskId, ex);
            return this.StatusCode(500, "An unexpected error occurred while retrieving available tags.");
            throw;
        }
    }

    /// <summary>
    /// Gets paginated available tags for a specific task.
    /// </summary>
    /// <param name="taskId">The unique identifier of the task.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of tags per page.</param>
    /// <returns>A paginated list of available tags.</returns>
    [HttpGet("Available/{taskId:int}/{pageNumber:min(1)}/{rowCount:min(1)}")]
    [Authorize]
    public async Task<ActionResult<List<TagDto>>> GetAvailableTagsPaginated(
        int taskId,
        int pageNumber,
        int rowCount)
    {
        try
        {
            var tags = await this.service.GetAvailableTaskTagsAsync(taskId, pageNumber, rowCount);

            if (tags == null || !tags.Any())
            {
                return this.Ok(new List<TagDto>());
            }

            var tagDtos = tags.Select(MapToDto.ToTagDto).ToList();

            TagLog.LogAvailableTagsRetrieved(this.logger, tagDtos.Count, taskId);

            return this.Ok(tagDtos);
        }
        catch (ArgumentOutOfRangeException ex) when (ex.ParamName == "pageNumber" || ex.ParamName == "rowCount")
        {
            TagLog.LogInvalidPaginationParameters(this.logger, pageNumber, rowCount);
            return this.BadRequest("Invalid pagination parameters. Page number and row count must be positive integers.");
        }
        catch (Exception ex)
        {
            TagLog.LogUnexpectedErrorRetrievingAvailableTags(this.logger, taskId, ex);
            return this.StatusCode(500, "An unexpected error occurred while retrieving available tags.");
            throw;
        }
    }

    /// <summary>
    /// Creates a new tag.
    /// </summary>
    /// <param name="dto">The data transfer object containing tag information.</param>
    /// <returns>The created tag.</returns>
    [HttpPut]
    [Authorize]
    public async Task<ActionResult<TagDto>> CreateTag([FromBody] CreateTagDto dto)
    {
        try
        {
            if (dto == null)
            {
                TagLog.LogInvalidTagData(this.logger, "Tag data is null");
                return this.BadRequest("Tag data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                TagLog.LogInvalidTagData(this.logger, "Tag title is empty");
                return this.BadRequest("Tag title is required.");
            }

            var model = new TagModel(0, dto.Title);
            var createdTag = await this.service.AddAsync(model);

            TagLog.LogTagCreated(this.logger, createdTag.Id);

            var tagDto = MapToDto.ToTagDto(createdTag);
            return this.CreatedAtAction(nameof(this.GetTag), new { tagId = createdTag.Id }, tagDto);
        }
        catch (EntityWithIdExistsException)
        {
            TagLog.LogTagAlreadyExists(this.logger, 0);
            return this.Conflict("A tag with this ID already exists.");
        }
        catch (ArgumentNullException ex)
        {
            TagLog.LogInvalidTagData(this.logger, ex.Message);
            return this.BadRequest("Invalid tag data provided.");
        }
        catch (UnableToCreateException ex)
        {
            TagLog.LogUnexpectedErrorCreatingTag(this.logger, ex);
            return this.StatusCode(500, "Unable to create tag. Please try again later.");
        }
        catch (Exception ex)
        {
            TagLog.LogUnexpectedErrorCreatingTag(this.logger, ex);
            return this.StatusCode(500, "An unexpected error occurred while creating the tag.");
            throw;
        }
    }

    /// <summary>
    /// Updates an existing tag.
    /// </summary>
    /// <param name="dto">The data transfer object containing updated tag information.</param>
    /// <returns>The updated tag.</returns>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<TagDto>> UpdateTag([FromBody] UpdateTagDto dto)
    {
        if (dto == null)
        {
            TagLog.LogInvalidTagData(this.logger, "Tag data is null");
            return this.BadRequest("Tag data is required.");
        }

        try
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                TagLog.LogInvalidTagData(this.logger, "Tag title is empty");
                return this.BadRequest("Tag title is required.");
            }

            var model = new TagModel(dto.Id, dto.Title);
            var updatedTag = await this.service.UpdateAsync(model);

            TagLog.LogTagUpdated(this.logger, dto.Id);

            var tagDto = MapToDto.ToTagDto(updatedTag);
            return this.Ok(tagDto);
        }
        catch (EntityNotFoundException)
        {
            TagLog.LogTagNotFound(this.logger, dto.Id);
            return this.NotFound($"Tag with ID {dto.Id} was not found.");
        }
        catch (ArgumentNullException ex)
        {
            TagLog.LogInvalidTagData(this.logger, ex.Message);
            return this.BadRequest("Invalid tag data provided.");
        }
        catch (UnableToUpdateException ex)
        {
            TagLog.LogUnableToUpdateTag(this.logger, dto.Id, ex.Message, ex);
            return this.StatusCode(500, "Unable to update tag. Please try again later.");
        }
        catch (Exception ex)
        {
            TagLog.LogUnexpectedErrorUpdatingTag(this.logger, dto.Id, ex);
            return this.StatusCode(500, "An unexpected error occurred while updating the tag.");
            throw;
        }
    }

    /// <summary>
    /// Deletes a specific tag by its ID.
    /// </summary>
    /// <param name="tagId">The unique identifier of the tag.</param>
    /// <returns>The result of the deletion operation.</returns>
    [HttpDelete("{tagId:int}")]
    [Authorize]
    public async Task<ActionResult> DeleteTag(int tagId)
    {
        try
        {
            await this.service.DeleteAsync(tagId);

            TagLog.LogTagDeleted(this.logger, tagId);

            return this.Ok(new { message = "Tag has been deleted successfully." });
        }
        catch (EntityNotFoundException)
        {
            TagLog.LogTagNotFound(this.logger, tagId);
            return this.NotFound($"Tag with ID {tagId} was not found.");
        }
        catch (UnableToDeleteException ex)
        {
            TagLog.LogUnableToDeleteTag(this.logger, tagId, ex.Message, ex);
            return this.StatusCode(500, "Unable to delete tag. Please try again later.");
        }
        catch (Exception ex)
        {
            TagLog.LogUnexpectedErrorDeletingTag(this.logger, tagId, ex);
            return this.StatusCode(500, "An unexpected error occurred while deleting the tag.");
            throw;
        }
    }
}
