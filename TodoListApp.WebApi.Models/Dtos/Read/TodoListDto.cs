namespace TodoListApp.WebApi.Models.Dtos.Read;

public class TodoListDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string OwnerName { get; set; } = string.Empty;
}
