using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Context;
using TaskManager.Api.Dtos;
using TaskManager.Api.Enums;
using TaskManager.Api.Models;

namespace TaskManager.Api.Controllers
{
    public class TasksController(AppDbContext db) : ControllerBase
    {
        [HttpGet("api/projects/{projectId:int}/tasks")]
        public async Task<ActionResult<List<TaskItemDto>>> GetByProject(

            int projectId,
            [FromQuery] TaskItemStatus? status,
            [FromQuery] bool overdue = false)
        {
            if (!await db.Projects.AnyAsync(p => p.Id == projectId))
            {
                return NotFound($"Project with ID {projectId} not found.");
            }
            var query = db.Tasks.AsQueryable();
            query = query.Where(t => t.ProjectId == projectId);
            if (status is not null)
            {
                query = query.Where(t => t.Status == status.Value);
            }
            if (overdue)
            {
                var now = DateTime.UtcNow;
                query = query.Where(t => t.DueDate.HasValue && t.DueDate.Value < now && t.Status != TaskItemStatus.Done);
            }
            var tasks = await query
                .OrderBy(t => t.DueDate.HasValue ? 0 : 1)
                .ThenBy(t => t.DueDate)
                .Select(t => new TaskItemDto(t.Id, t.Title, t.Status, t.DueDate, t.ProjectId))
                .ToListAsync();
            return tasks;
        }

        [HttpGet("api/tasks/{id:int}")]
        public async Task<ActionResult<TaskItemDto>> GetById(int id)
        {
            var task = await db.Tasks
                .AsNoTracking()
                .Where(t => t.Id == id)
                .Select(t => new TaskItemDto(t.Id, t.Title, t.Status, t.DueDate, t.ProjectId))
                .FirstOrDefaultAsync();
            if (task is null)
            {
                return NotFound($"Task with ID {id} not found.");
            }
            return task;
        }

        [HttpPost("api/projects/{projectId:int}/tasks")]
        public async Task<ActionResult<TaskItemDto>> Create(int projectId, CreateTaskRequest request)
        {
            if (!await db.Projects.AnyAsync(p => p.Id == projectId))
            {
                return NotFound($"Project with ID {projectId} not found.");
            }
            var task = new TaskItem
            {
                Title = request.Title,
                DueDate = request.DueDate,
                ProjectId = projectId
            };
            db.Tasks.Add(task);
            await db.SaveChangesAsync();
            var taskDto = new TaskItemDto(task.Id, task.Title, task.Status, task.DueDate, task.ProjectId);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, taskDto);
        }

        [HttpPut("api/tasks/{id:int}")]
        public async Task<ActionResult<TaskItemDto>> Update(int id, UpdateTaskRequest request)
        {
            var task = await db.Tasks.FindAsync(id);
            if (task is null)
            {
                return NotFound($"Task with ID {id} not found.");
            }
            task.Title = request.Title;
            task.Status = request.Status;
            task.DueDate = request.DueDate;
            await db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("api/tasks/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await db.Tasks.FindAsync(id);
            if (task is null)
            {
                return NotFound($"Task with ID {id} not found.");
            }
            db.Tasks.Remove(task);
            await db.SaveChangesAsync();
            return NoContent();
        }
    }
}
