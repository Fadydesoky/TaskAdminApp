using Microsoft.EntityFrameworkCore;
using TaskAdmin.Models;

namespace TaskAdmin.Data
{
    public class TodoContext : DbContext
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
            Database.EnsureCreated();
        }

        public DbSet<TodoTask> TodoTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoTask>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TaskName).IsRequired();
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.ListType).IsRequired().HasDefaultValue("Default");
                entity.Property(e => e.IsCompleted).HasDefaultValue(false);
            });
        }
    }
}
