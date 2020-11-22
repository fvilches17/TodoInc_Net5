using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoInc.Server.Persistence.Entities;

namespace TodoInc.Server.Persistence
{
    public interface ITodosRepository
    {
        Task<IEnumerable<Todo>> GetTodosAsync(Func<Todo, bool>? filter = null);
        Task<Todo?> GetTodoAsync(int todoId);
        Task<Todo> CreateTodoAsync(string title, string? description = null);
        Task<bool> UpdateTodoAsync(Todo todo);
        Task<bool> DeleteTodoAsync(int id);
    }
}
