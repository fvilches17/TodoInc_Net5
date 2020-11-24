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

        public async Task<TodoRecord?> GetTodoByIdAsync(int id)
        {
            var entity = await _todosRepository.GetTodoAsync(id);
            return entity is null ? null : new TodoRecord(entity.Id, entity.Title, entity.Description, entity.IsComplete);
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

        public async Task<OperationStatus> FullUpdateTodoAsync(int id, TodoEditModel editModel)
        {
            var todo = await _todosRepository.GetTodoAsync(id);

            if (todo is null)
            {
                return OperationStatus.EntityNotFound;
            }

            todo.Title = editModel.Title;
            todo.Description = editModel.Description;

            if (await _todosRepository.UpdateTodoAsync(todo))
            {
                return OperationStatus.Success;
            }

            return OperationStatus.UnexpectedResult;
        }

        public async Task<TodoRecord> CreateTodoAsync(TodoCreateModel model)
        {
            var newEntity = await _todosRepository.CreateTodoAsync(model.Title, model.Description);
            return new TodoRecord(newEntity.Id, newEntity.Title, newEntity.Description, IsComplete: false);
        }

        public async Task<OperationStatus> DeleteTodoAsync(int id)
        {
            if (await _todosRepository.DeleteTodoAsync(id))
            {
                return OperationStatus.Success;
            }

            return OperationStatus.EntityNotFound;
        }
    }
}
