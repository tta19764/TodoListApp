using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;
using TodoListApp.WebApi.Models.Dtos.Create;
using TodoListApp.WebApi.Models.Dtos.Read;

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

    // GET: api/<TodoListsController>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<TodoListDto>>> Get()
    {
        var userNameIdentifier = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userNameIdentifier is null ||
        !int.TryParse(userNameIdentifier, NumberStyles.Integer, CultureInfo.InvariantCulture, out var userId))
        {
            return this.Unauthorized("Invalid user identifier.");
        }

        var todoLists = await this.service.GetAllByUserAsync(userId);

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
    [HttpPut]
    public async Task<ActionResult<TodoListDto>> Put([FromBody] CreateTodoListDto dto)
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
}
