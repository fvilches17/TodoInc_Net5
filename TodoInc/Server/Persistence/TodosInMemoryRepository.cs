using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoInc.Server.Persistence.Entities;

namespace TodoInc.Server.Persistence
{
    public class TodosInMemoryRepository : ITodosRepository
    {
        public TodosInMemoryRepository()
        {
            var seedData = new[]
            {
                new KeyValuePair<int,Todo>(1, new Todo("Water the plants") {Id = 1, Description = "Use mineral water"}),
                new KeyValuePair<int,Todo>(2, new Todo("Pay electricity bill") {Id = 2, IsComplete = true}),
                new KeyValuePair<int,Todo>(3, new Todo("Feed Mr. Meow") {Id = 3, Description = "Include treat"}),
                new KeyValuePair<int,Todo>(4, new Todo("Submit report") {Id = 4, DeletedDateUtc = new DateTime(2020,01,01)})
            };

            Store = new ConcurrentDictionary<int, Todo>(seedData);
        }

        public ConcurrentDictionary<int, Todo> Store { get; }

        public Task<IEnumerable<Todo>> GetTodosAsync(Func<Todo, bool>? filter = null)
        {
            if (filter is null) return Task.FromResult(Store.Values.AsEnumerable());
            return Task.FromResult(Store.Values.Where(filter));
        }

        public Task<Todo?> GetTodoAsync(int todoId)
        {
            Store.TryGetValue(key: todoId, out var todo);
            return Task.FromResult(todo);
        }

        public Task<Todo> CreateTodoAsync(string title, string? description = null)
        {
            var todo = Store.GetOrAdd(Store.Count, key => new Todo(title) { Id = ++key, Description = description });
            return Task.FromResult(todo);
        }

        public Task<bool> UpdateTodoAsync(Todo todo) => Task.FromResult(Store.TryUpdate(todo.Id, todo, todo));

        public Task<bool> DeleteTodoAsync(int id)
        {
            var todoExists = Store.TryGetValue(id, out var todo);
            if (!todoExists) return Task.FromResult(false);
            todo!.DeletedDateUtc = DateTime.UtcNow;
            return Task.FromResult(Store.TryUpdate(id, todo, todo));
        }
    }
}
