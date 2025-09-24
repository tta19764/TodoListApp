using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;
using TodoListApp.WebApi.Models.Dtos.Create;
using TodoListApp.WebApi.Models.Dtos.Read;
using TodoListApp.WebApi.Models.Dtos.Update;

namespace TodoListApp.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoListsController : ControllerBase
{
    private readonly ITodoListService service;

    public TodoListsController(ITodoListService todoListService)
    {
        this.service = todoListService;
    }

    [Authorize]
    [HttpGet("{listId:int}")]
    public async Task<ActionResult<TodoListDto>> GetList(int listId)
    {
        var userNameIdentifier = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userNameIdentifier is null ||
        !int.TryParse(userNameIdentifier, NumberStyles.Integer, CultureInfo.InvariantCulture, out var userId))
        {
            return this.Unauthorized("Invalid user identifier.");
        }

        var todoList = await this.service.GetByIdAsync(listId, userId);

        if (todoList is null)
        {
            return this.NotFound("The list was not found.");
        }

        return this.Ok(new TodoListDto()
        {
            Id = todoList.Id,
            Title = todoList.Title,
            Description = todoList.Description,
            OwnerName = $"{todoList.ListOwner!.FirstName} {todoList.ListOwner.LastName[0]}.",
        });
    }

    [Authorize]
    [HttpGet("{pageNumber:min(1)}/{rowCount:min(1)}")]
    public async Task<ActionResult<List<TodoListDto>>> GetLists(int pageNumber, int rowCount)
    {
        var userNameIdentifier = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userNameIdentifier is null ||
        !int.TryParse(userNameIdentifier, NumberStyles.Integer, CultureInfo.InvariantCulture, out var userId))
        {
            return this.Unauthorized("Invalid user identifier.");
        }

        var todoLists = await this.service.GetAllByUserAsync(userId, pageNumber, rowCount);

        return this.Ok(todoLists
            .Select(l => new TodoListDto()
            {
                Id = l.Id,
                Title = l.Title,
                Description = l.Description,
                OwnerName = $"{l.ListOwner!.FirstName} {l.ListOwner.LastName[0]}.",
            })
            .ToList());
    }

    [Authorize]
    [HttpGet("UserLists/{pageNumber:min(1)}/{rowCount:min(1)}")]
    public async Task<ActionResult<List<TodoListDto>>> GetUsersLists(int pageNumber, int rowCount)
    {
        var userNameIdentifier = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userNameIdentifier is null ||
        !int.TryParse(userNameIdentifier, NumberStyles.Integer, CultureInfo.InvariantCulture, out var userId))
        {
            return this.Unauthorized("Invalid user identifier.");
        }

        var todoLists = await this.service.GetAllByAuthorAsync(userId, pageNumber, rowCount);

        return this.Ok(todoLists
            .Select(l => new TodoListDto()
            {
                Id = l.Id,
                Title = l.Title,
                Description = l.Description,
                OwnerName = $"{l.ListOwner!.FirstName} {l.ListOwner.LastName[0]}.",
            })
            .ToList());
    }

    [Authorize]
    [HttpGet("Delete/{listId:int}")]
    public async Task<ActionResult> DeleteuserList(int listId)
    {
        var userNameIdentifier = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userNameIdentifier is null ||
        !int.TryParse(userNameIdentifier, NumberStyles.Integer, CultureInfo.InvariantCulture, out var userId))
        {
            return this.Unauthorized("Invalid user identifier.");
        }

        var todoList = await this.service.GetByIdAsync(listId);

        if (todoList is null)
        {
            return this.NotFound("The list is not found");
        }
        else if (todoList.OwnerId != userId)
        {
            return this.Unauthorized("The user is not the list owner.");
        }

        await this.service.DeleteAsync(listId);

        return this.Ok("The list has been deleted.");
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<TodoListDto>> CreateList([FromBody] CreateTodoListDto dto)
    {
        if (dto is null)
        {
            return this.BadRequest();
        }

        var userNameIdentifier = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userNameIdentifier is null ||
        !int.TryParse(userNameIdentifier, NumberStyles.Integer, CultureInfo.InvariantCulture, out var userId))
        {
            return this.Unauthorized("Invalid user identifier.");
        }

        var model = new TodoListModel(0, userId, dto.Title, dto.Description);

        var newEntity = await this.service.AddAsync(model);
        string ownerName = (newEntity.ListOwner is null) ? "N/A" : $"{newEntity.ListOwner.FirstName} {newEntity.ListOwner.LastName[0]}.";
        var result = new TodoListDto()
        {
            Id = newEntity.Id,
            Title = newEntity.Title,
            Description = newEntity.Description,
            OwnerName = ownerName,
        };

        return this.Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<TodoListDto>> UpadateList([FromBody] UpdateTodoListDto dto)
    {
        if (dto is null)
        {
            return this.BadRequest();
        }

        var userNameIdentifier = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userNameIdentifier is null ||
        !int.TryParse(userNameIdentifier, NumberStyles.Integer, CultureInfo.InvariantCulture, out var userId))
        {
            return this.Unauthorized("Invalid user identifier.");
        }

        var model = new TodoListModel(dto.Id, userId, dto.Title, dto.Description);

        var newEntity = await this.service.UpdateAsync(model);
        string ownerName = (newEntity.ListOwner is null) ? "N/A" : $"{newEntity.ListOwner.FirstName} {newEntity.ListOwner.LastName[0]}.";
        var result = new TodoListDto()
        {
            Id = newEntity.Id,
            Title = newEntity.Title,
            Description = newEntity.Description,
            OwnerName = ownerName,
        };

        return this.Ok(result);
    }
}
