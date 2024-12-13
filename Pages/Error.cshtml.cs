using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskAdmin.Data;
using TaskAdmin.Models;

namespace TaskAdmin.Pages.Todo
{
    public class IndexModel(TodoContext context) : PageModel
    {
        private readonly TodoContext _context = context;

        public required List<TodoTask> Tasks { get; set; }

        public void OnGet()
        {
            Tasks = _context.TodoTasks.ToList();
        }

        // إضافة مهمة جديدة
        public async Task<IActionResult> OnPostAsync(string taskName)
        {
            if (!string.IsNullOrEmpty(taskName))
            {
                var newTask = new TodoTask
                {
                    TaskName = taskName,
                    IsCompleted = false
                };

                _context.TodoTasks.Add(newTask);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        // حذف مهمة
        public async Task<IActionResult> OnGetDeleteTaskAsync(int id)
        {
            var task = await _context.TodoTasks.FindAsync(id);
            if (task != null)
            {
                _context.TodoTasks.Remove(task);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        // تحديث حالة المهمة
        public async Task<IActionResult> OnGetToggleTaskStatusAsync(int id)
        {
            var task = await _context.TodoTasks.FindAsync(id);
            if (task != null)
            {
                task.IsCompleted = !task.IsCompleted;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}