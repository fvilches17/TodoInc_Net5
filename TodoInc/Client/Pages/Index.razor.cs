using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TodoInc.Client.Components;
using TodoInc.Shared;

namespace TodoInc.Client.Pages
{
    public partial class Index
    {
        private IEnumerable<TodoRecord>? _todoRecords;
        private bool _showCompletedTodos = true;

        [Inject]
        public HttpClient ApiClient { get; set; } = null!;

        public TodoQuickAddModal TodoQuickAddModal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadTodosAsync();
        }

        private async Task LoadTodosAsync()
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

        private async Task TodoQuickAdd_OnDialogClose()
        {
            await LoadTodosAsync();
            TodoQuickAddModal.Close();
            StateHasChanged();
        }


        private void ShowTodoQuickAddDialog()
        {
            TodoQuickAddModal.Show();
        }
    }
}
