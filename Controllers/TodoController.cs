using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskAdmin.Data;
using TaskAdmin.Models;

namespace TaskAdmin.Controllers
{
    public class TodoController : Controller
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;
        }

        // عرض المهام
        public async Task<IActionResult> Index()
        {
            var tasks = await _context.TodoTasks.ToListAsync();
            return View(tasks);
        }

        // إضافة مهمة جديدة
        [HttpPost]
        public async Task<IActionResult> AddTask(string taskName)
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

            return RedirectToAction(nameof(Index));
        }

        // حذف مهمة
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.TodoTasks.FindAsync(id);
            if (task != null)
            {
                _context.TodoTasks.Remove(task);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // تحديث حالة المهمة (إنهاء/عدم إنهاء)
        public async Task<IActionResult> ToggleTaskStatus(int id)
        {
            var task = await _context.TodoTasks.FindAsync(id);
            if (task != null)
            {
                task.IsCompleted = !task.IsCompleted;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
