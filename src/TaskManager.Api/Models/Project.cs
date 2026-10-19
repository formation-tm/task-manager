namespace TaskManager.Api.Models
{
    public class Project
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; } = null!;
        public List<TaskItem> Tasks { get; set; } = [];
    }
}
