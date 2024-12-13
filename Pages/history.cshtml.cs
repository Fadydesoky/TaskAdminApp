using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskAdmin.Models;
using TaskAdmin.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskAdmin.Pages
{
    public class HistoryModel : PageModel
    {
        private readonly ITodoService _todoService;

        // بيانات العرض
        public List<TodoTask> Tasks { get; set; } = new List<TodoTask>();
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int IncompleteTasks { get; set; }

        // Constructor to inject ITodoService
        public HistoryModel(ITodoService todoService)
        {
            _todoService = todoService;
        }

        // تحميل البيانات في OnGetAsync
        public async Task OnGetAsync()
        {
            // جلب جميع المهام
            Tasks = await _todoService.GetAllTasksAsync();

            // حساب إجمالي المهام المكتملة وغير المكتملة
            TotalTasks = Tasks.Count;
            CompletedTasks = Tasks.Count(t => t.IsCompleted);
            IncompleteTasks = Tasks.Count(t => !t.IsCompleted);
        }
    }
}
