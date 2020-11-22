using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoInc.Server.Biz;
using TodoInc.Shared;

namespace TodoInc.Server.Controllers
{
    [ApiController]
    [Route("todos")]
    public class TodosController : ControllerBase
    {
        private readonly TodosService _todosService;

        public TodosController(TodosService todosService)
        {
            _todosService = todosService;
        }

        [HttpGet]
        public async Task<IEnumerable<TodoRecord>> GetTodos() => await _todosService.GetTodosAsync();
    }
}
