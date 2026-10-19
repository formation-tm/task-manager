using TaskManager.Api.Enums;

namespace TaskManager.Api.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public TaskItemStatus Status { get; set; } = TaskItemStatus.Todo;
        public DateTime? DueDate { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
    }
}
