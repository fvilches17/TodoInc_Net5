using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TodoInc.Shared;

namespace TodoInc.Client.Pages
{
    public partial class TodoEdit
    {
        [Parameter]
        public int TodoId { get; set; }

        [Inject]
        public HttpClient ApiClient { get; set; }

        [Inject]
        public NavigationManager NavManager { get; set; }

        public TodoEditModel TodoEditModel { get; set; } = new TodoEditModel();

        protected override async Task OnInitializedAsync()
        {
            TodoEditModel = await ApiClient.GetFromJsonAsync<TodoEditModel>($"api/todos/{TodoId}");
        }

        public async Task HandleOnValidSubmitAsync()
        {
            await ApiClient.PostAsJsonAsync($"api/todos/{TodoId}", TodoEditModel);
            NavManager.NavigateTo("/");
        }
    }
}
