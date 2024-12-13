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
    }
}

