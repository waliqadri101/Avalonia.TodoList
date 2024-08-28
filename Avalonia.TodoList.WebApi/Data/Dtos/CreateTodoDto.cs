using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Avalonia.TodoList.WebApi.Data.Dtos
{
    public class CreateTodoDto
    {
        [Required]
        [MinLength(2), MaxLength(280, ErrorMessage = "Todo name must be between 2-280 characters.")]
        public string Name { get; set; } = string.Empty;

        // This property is nullable because it's optional and does not need to be provided while creating a new Todo.
        // If not provided or set to null, it will default to false when processed.
        public bool? IsCompleted { get; set; } = false;

        [JsonIgnore]
        public DateTime? CreatedOn { get; set; }
        
        [JsonIgnore]
        public string? CreatedBy { get; set; }
    }
}
