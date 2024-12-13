using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskAdmin.Models;

namespace TaskAdmin.Services
{
    public interface ITodoService
    {
        Task<List<TodoTask>> GetAllTasksAsync();
        Task<List<TodoTask>> GetTasksByDateAndTypeAsync(DateTime date, string listType);
        Task<TodoTask> AddTaskAsync(string taskName, DateTime date, string listType);
        Task<bool> DeleteTaskAsync(int id);
        Task<bool> ToggleTaskStatusAsync(int id);
        Task<List<string>> GetUniqueListTypesAsync();
    }
}

