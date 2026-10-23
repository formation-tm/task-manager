namespace TaskManager.Api.Dtos
{
    public record ProjectDetailsDto(int Id, string Name, string? Description, int OwnerId, List<TaskItemDto> Tasks);
}
