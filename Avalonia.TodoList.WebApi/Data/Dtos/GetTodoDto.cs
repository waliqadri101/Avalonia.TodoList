namespace Avalonia.TodoList.WebApi.Data.Dtos
{
    public class GetTodoDto
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string ShortName
        {
            get
            {
                if (Name.Length > 30)
                {
                    return Name.Substring(0, 29);
                }
                else
                {
                    return Name;
                }
            }
        }
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = string.Empty;
    }
}
