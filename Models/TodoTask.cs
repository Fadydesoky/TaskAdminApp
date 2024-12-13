using System;
using System.ComponentModel.DataAnnotations;

namespace TaskAdmin.Models
{
    public class TodoTask
    {
        public int Id { get; set; }
        
        [Required]
        public string? TaskName { get; set; }
        
        public bool IsCompleted { get; set; }
        
        [Required]
        public DateTime Date { get; set; } = DateTime.Today;
        
        [Required]
        public string ListType { get; set; } = "Default";

        private Task? completedTask;

        public Task GetCompletedTask()
        {
#pragma warning disable CS8603 // Possible null reference return.
            return completedTask;
#pragma warning restore CS8603 // Possible null reference return.
        }

        internal void SetCompletedTask(Task value)
        {
            completedTask = value;
        }
    }
}
