using System;
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

        public async Task<IEnumerable<TodoRecord>> GetTodosAsync(bool includeCompleted)
        {
            var todoEntities = await _todosRepository.GetTodosAsync(todo => todo.DeletedDateUtc is null && (includeCompleted || !todo.IsComplete));

            return todoEntities
                .OrderBy(x => x.IsComplete)
                .Select(entity => new TodoRecord(entity.Id, entity.Title, entity.Description, entity.IsComplete));
        }

        public async Task<OperationStatus> ToggleTodoCompletedStatusAsync(int id)
        {
            var todo = await _todosRepository.GetTodoAsync(id);

            if (todo is null)
            {
                return OperationStatus.EntityNotFound;
            }

            todo.IsComplete = !todo.IsComplete;

            if (await _todosRepository.UpdateTodoAsync(todo))
            {
                return OperationStatus.Success;
            }

            return OperationStatus.UnexpectedResult;
        }
    }
}
