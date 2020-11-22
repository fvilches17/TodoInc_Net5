using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoInc.Server.Persistence;
using TodoInc.Shared;

namespace TodoInc.Server.Biz
{
    public class TodosService
    {
        private readonly ITodosRepository _todosRepository;

        public TodosService(ITodosRepository todosRepository)
        {
            _todosRepository = todosRepository;
        }

        public async Task<IEnumerable<TodoRecord>> GetTodosAsync()
        {
            var todoEntities = await _todosRepository.GetTodosAsync(todo => todo.DeletedDateUtc is null);
            return todoEntities.Select(entity => new TodoRecord(entity.Id, entity.Title, entity.Description, entity.IsComplete));
        }
    }
}
