using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoInc.Server.Biz;
using TodoInc.Shared;

namespace TodoInc.Server.Controllers
{
    [ApiController]
    [Route("api/todos")]
    public class TodosController : ControllerBase
    {
        private readonly TodosService _todosService;

        public TodosController(TodosService todosService)
        {
            _todosService = todosService;
        }

        [HttpGet]
        public async Task<IEnumerable<TodoRecord>> GetTodos(bool includeCompleted = true) => await _todosService.GetTodosAsync(includeCompleted);

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTodoById(int id)
        {
            var record = await _todosService.GetTodoByIdAsync(id);

            if (record is null)
            {
                return NotFound();
            }

            return Ok(record);
        }


        [HttpPost("{id:int}/toggleCompletedStatus")]
        public async Task<IActionResult> CompleteTodo(int id) => await _todosService.ToggleTodoCompletedStatusAsync(id) switch
        {
            OperationStatus.Success => NoContent(),
            OperationStatus.EntityNotFound => NotFound(),
            _ => throw new Exception($"Unexpected error while attempting to complete todo with id '{id}'")
        };

        [HttpPost("{id:int}")]
        public async Task<IActionResult> UpdateTodo([FromRoute] int id, TodoEditModel editModel) => await _todosService.FullUpdateTodoAsync(id, editModel) switch
        {
            OperationStatus.Success => NoContent(),
            OperationStatus.EntityNotFound => NotFound(),
            _ => throw new Exception($"Unexpected error while attempting to complete todo with id '{id}'")
        };

    }
}
