namespace TaskManager.Api.Dtos
{
    public record CreateTaskRequest(string Title, DateTime? DueDate);

}
