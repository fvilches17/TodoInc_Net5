using System;

namespace TodoInc.Server.Persistence.Entities
{
    public class Todo
    {
        public Todo(string title)
        {
            Title = title;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsComplete { get; set; }
        public DateTime? DeletedDateUtc { get; set; }
    }
}
