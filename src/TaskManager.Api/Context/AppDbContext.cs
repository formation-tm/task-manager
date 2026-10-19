using TaskManager.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Api.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<User>(e =>
            {
                e.Property(u => u.Name).HasMaxLength(100);
                e.Property(u => u.Email).HasMaxLength(200);
                e.HasIndex(u => u.Email).IsUnique();
            });

            b.Entity<Project>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(150);
                e.HasOne(p => p.Owner).WithMany(u => u.Projects)
                 .HasForeignKey(p => p.OwnerId).OnDelete(DeleteBehavior.Restrict);
            });

            b.Entity<TaskItem>(e =>
            {
                e.Property(t => t.Title).HasMaxLength(200);
                e.Property(t => t.Status).HasConversion<string>().HasMaxLength(20);
                e.HasOne(t => t.Project).WithMany(p => p.Tasks)
                 .HasForeignKey(t => t.ProjectId).OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
