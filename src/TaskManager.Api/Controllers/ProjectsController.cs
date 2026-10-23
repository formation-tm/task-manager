using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Context;
using TaskManager.Api.Dtos;
using TaskManager.Api.Models;

namespace TaskManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController(AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<ProjectDto>>> GetAll()
        {
            var projects = await context.Projects
                .AsNoTracking()
                .Select(p => new ProjectDto(p.Id, p.Name, p.Description, p.OwnerId))
                .ToListAsync();
            return projects;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProjectDto>> GetById(int id)
        {
            var project = await context.Projects
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProjectDto(p.Id, p.Name, p.Description, p.OwnerId))
                .FirstOrDefaultAsync();

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> Create(CreateProjectRequest request)
        {
            if(!await context.Users.AnyAsync(u => u.Id == request.OwnerId))
            {
                return BadRequest($"User with Id {request.OwnerId} does not exist.");
            }
            var project = new Project
            {
                Name = request.Name,
                Description = request.Description,
                OwnerId = request.OwnerId
            };
            context.Projects.Add(project);
            await context.SaveChangesAsync();
            var projectDto = new ProjectDto(project.Id, project.Name, project.Description, project.OwnerId);
            return CreatedAtAction(nameof(GetById), new { id = project.Id }, projectDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProjectDto>> Update(int id, UpdateProjectRequest request)
        {
            var project = await context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            project.Name = request.Name;
            project.Description = request.Description;
            await context.SaveChangesAsync();
            // var projectDto = new ProjectDto(project.Id, project.Name, project.Description, project.OwnerId);
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            context.Projects.Remove(project);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id:int}/details")]
        public async Task<ActionResult<ProjectDetailsDto>> GetProjectDetails(int id)
        {
            var project = await context.Projects
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProjectDetailsDto(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.OwnerId,
                    p.Tasks.Select(t => new TaskItemDto(t.Id, t.Title, t.Status, t.DueDate, t.ProjectId)).ToList()
                ))
                .FirstOrDefaultAsync();
            if (project is null)
            {
                return NotFound();
            }
            return project;
        }
    }
}