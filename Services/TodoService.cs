using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskAdmin.Data;
using TaskAdmin.Models;

namespace TaskAdmin.Services
{
    public class TodoService : ITodoService
    {
        private readonly TodoContext _context;

        public TodoService(TodoContext context)
        {
            _context = context;
        }

        public async Task<List<TodoTask>> GetAllTasksAsync()
        {
            return await _context.TodoTasks.OrderByDescending(t => t.Date).ToListAsync();
        }

        public async Task<List<TodoTask>> GetTasksByDateAndTypeAsync(DateTime date, string listType)
        {
            return await _context.TodoTasks
                .Where(t => t.Date.Date == date.Date && t.ListType == listType)
                .OrderBy(t => t.IsCompleted)
                .ThenBy(t => t.TaskName)
                .ToListAsync();
        }

        public async Task<TodoTask> AddTaskAsync(string taskName, DateTime date, string listType)
        {
            var newTask = new TodoTask
            {
                TaskName = taskName,
                IsCompleted = false,
                Date = date,
                ListType = listType
            };

            _context.TodoTasks.Add(newTask);
            await _context.SaveChangesAsync();
            return newTask;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.TodoTasks.FindAsync(id);
            if (task == null)
                return false;

            _context.TodoTasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleTaskStatusAsync(int id)
        {
            var task = await _context.TodoTasks.FindAsync(id);
            if (task == null)
                return false;

            task.IsCompleted = !task.IsCompleted;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<string>> GetUniqueListTypesAsync()
        {
            return await _context.TodoTasks
                .Select(t => t.ListType)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();
        }

        public Task GetTaskByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task GetCompletedTasksAsync()
        {
            throw new NotImplementedException();
        }

        public Task GetIncompleteTasksAsync()
        {
            throw new NotImplementedException();
        }
    }
}
