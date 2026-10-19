namespace TaskManager.Api.Dtos
{
    public record ProjectDto
    (
        int Id, 
        string Name, 
        string? Description, 
        int OwnerId
    );
}
