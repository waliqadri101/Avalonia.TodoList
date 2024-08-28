using System.ComponentModel.DataAnnotations;

namespace Avalonia.TodoList.WebApi.Data.Entities
{
    public class Todo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(2), MaxLength(280, ErrorMessage = "Todo name must be between 2-280 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = string.Empty;
    }
}
