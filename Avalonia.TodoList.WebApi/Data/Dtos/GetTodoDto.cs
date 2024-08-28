namespace Avalonia.TodoList.WebApi.Data.Dtos
{
    public class GetTodoDto
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = string.Empty;
    }
}
