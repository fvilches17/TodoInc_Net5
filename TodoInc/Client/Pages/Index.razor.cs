using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TodoInc.Shared;

namespace TodoInc.Client.Pages
{
    public partial class Index
    {
        private IEnumerable<TodoRecord>? _todoRecords;
        private bool _showCompletedTodos = true;

        [Inject]
        public HttpClient ApiClient { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            _todoRecords = await ApiClient.GetFromJsonAsync<IEnumerable<TodoRecord>>("api/todos");
        }

        private async Task ToggleTodoCompletedStatusAsync(int todoId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"api/todos/{todoId}/toggleCompletedStatus");
            await ApiClient.SendAsync(request);
        }

        private async Task ToggleCompletedTodosVisibility()
        {
            _showCompletedTodos = !_showCompletedTodos;
            _todoRecords = await ApiClient.GetFromJsonAsync<IEnumerable<TodoRecord>>($"api/todos?includeCompleted={_showCompletedTodos}");
        }
    }
}
