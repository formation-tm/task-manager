using TaskManager.Api.Enums;

namespace TaskManager.Api.Dtos
{
    public record TaskItemDto(int Id, string Title, TaskItemStatus Status, DateTime? DueDate, int ProjectId);

}
