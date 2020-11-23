using System.ComponentModel.DataAnnotations;

namespace TodoInc.Shared
{
    public class TodoEditModel
    {
        [Required]
        public string Title { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }
    }
}
