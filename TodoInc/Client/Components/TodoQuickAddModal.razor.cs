using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TodoInc.Shared;

namespace TodoInc.Client.Components
{
    public partial class TodoQuickAddModal
    {
        [Inject]
        public HttpClient ApiClient { get; set; }

        public bool ShowModal { get; set; }

        [Parameter]
        public Func<Task> CallbackAction { get; set; }

        public TodoCreateModel TodoModel { get; set; } = new();

        public void Show()
        {
            ShowModal = true;
            TodoModel = new();
        }

        public void Close()
        {
            ShowModal = false;
            TodoModel = new();
        }

        private async Task HandleOnValidSubmitAsync()
        {
            var response = await ApiClient.PostAsJsonAsync("api/todos", TodoModel);

            if (response.IsSuccessStatusCode)
            {
                await CallbackAction.Invoke();
            }
            else
            {
                throw new Exception("Something went wrong, could not create todo");
            }
        }
    }
}
