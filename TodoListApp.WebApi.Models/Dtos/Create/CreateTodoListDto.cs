namespace TodoListApp.WebApi.Models.Dtos.Create;
public class CreateTodoListDto
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
