using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using TodoInc.Shared;

namespace TodoInc.Client.Components
{
    public partial class TodoCard
    {
        [Parameter]
        public TodoRecord TodoRecord { get; set; } = null!;

        [Parameter]
        public Func<int, Task> OnCheckboxTicked { get; set; } = null!;

        [Parameter]
        public Func<int, Task> OnTodoDeleteButtonClick { get; set; } = null!;

        private string CheckBoxId => $"cbx-{TodoRecord.Id}";


        protected override void OnInitialized()
        {
            if (TodoRecord == null)
            {
                throw new ArgumentException($"'{nameof(TodoCard)}' component initialized with empty param", nameof(TodoRecord));
            }
        }

        private async Task InvokeOnCheckboxTickedAsync()
        {
            await OnCheckboxTicked.Invoke(TodoRecord.Id);
            TodoRecord = TodoRecord with { IsComplete = !TodoRecord.IsComplete };
        }

        private async Task InvokeOnTodoDeleteButtonClickedAsync() => await OnTodoDeleteButtonClick.Invoke(TodoRecord.Id);
    }
}
