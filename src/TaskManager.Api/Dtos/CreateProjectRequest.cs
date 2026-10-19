namespace TaskManager.Api.Dtos
{
    public record CreateProjectRequest(string Name, string? Description, int OwnerId);
}
