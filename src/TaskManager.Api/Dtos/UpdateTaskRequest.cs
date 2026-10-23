using TaskManager.Api.Enums;

namespace TaskManager.Api.Dtos
{
    public record UpdateTaskRequest(string Title, TaskItemStatus Status, DateTime? DueDate);

}
