using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Avalonia.TodoList.Data.Dtos
{
    public class UpdateTodoDto
    {
        [Required]
        public int Id { get; set; } = 0;

        [Required]
        [MinLength(2), MaxLength(280, ErrorMessage = "Todo name must be between 2-280 characters.")]
        public string Name { get; set; } = string.Empty;

        public bool? IsCompleted { get; set; } = false;
    }
}
