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

        [HttpPost("{id}/toggleCompletedStatus")]
        public async Task<IActionResult> CompleteTodo(int id) => await _todosService.ToggleTodoCompletedStatusAsync(id) switch
        {
            OperationStatus.Success => NoContent(),
            OperationStatus.EntityNotFound => NotFound(),
            _ => throw new Exception($"Unexpected error while attempting to complete todo with id '{id}'")
        };
    }
}
