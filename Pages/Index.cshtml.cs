using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskAdmin.Models;
using TaskAdmin.Services;

namespace TaskAdmin.Pages.Todo
{
    public class TodoPageModel(ITodoService todoService) : PageModel
    {
        private readonly ITodoService _todoService = todoService;
        private static readonly DateTime MinDate = DateTime.Today.AddYears(-10);
        private static readonly DateTime MaxDate = DateTime.Today.AddYears(5);

        [BindProperty]
        public TodoTask Task { get; set; } = new();

        public List<TodoTask> TodoTasks { get; set; } = new();
        public List<string> ListTypes { get; set; } = new();
        public required string CurrentListType { get; set; }
        public DateTime CurrentDate { get; set; }

        public async Task OnGetAsync(string? listType = null, DateTime? date = null)
        {
            // Validate and normalize the date
            CurrentDate = NormalizeDate(date ?? DateTime.Today);
            ListTypes = await _todoService.GetUniqueListTypesAsync();
            CurrentListType = listType ?? (ListTypes.Count > 0 ? ListTypes[0] : "Default");

            TodoTasks = await _todoService.GetTasksByDateAndTypeAsync(CurrentDate, CurrentListType);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(Task.TaskName))
            {
                return Page();
            }

            // Validate task date before saving
            Task.Date = NormalizeDate(Task.Date);
            await _todoService.AddTaskAsync(Task.TaskName, Task.Date, Task.ListType);
            return RedirectToPage(new { listType = Task.ListType, date = Task.Date.ToString("yyyy-MM-dd") });
        }

        // Delete a single task
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _todoService.DeleteTaskAsync(id);
            return RedirectToPage(new { listType = CurrentListType, date = DateTime.Today.ToString("yyyy-MM-dd") });
        }

        // Toggle task status (Completed / Pending)
        public async Task<IActionResult> OnPostToggleStatusAsync(int id)
        {
            await _todoService.ToggleTaskStatusAsync(id);
            return RedirectToPage(new { listType = CurrentListType, date = DateTime.Today.ToString("yyyy-MM-dd") });
        }

        // Delete all tasks for the current list and date
        private DateTime NormalizeDate(DateTime date)
        {
            return date.Date.Clamp(MinDate, MaxDate);
        }

        public DateTime GetPreviousDay()
        {
            return CurrentDate > MinDate ? CurrentDate.AddDays(-1) : MinDate;
        }

        public DateTime GetNextDay()
        {
            return CurrentDate < MaxDate ? CurrentDate.AddDays(1) : MaxDate;
        }
    }

    public static class DateTimeExtensions
    {
        public static DateTime Clamp(this DateTime date, DateTime min, DateTime max)
        {
            return date < min ? min : (date > max ? max : date);
        }
    }
}