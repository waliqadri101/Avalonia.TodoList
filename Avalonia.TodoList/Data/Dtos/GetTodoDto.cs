using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel;

namespace Avalonia.TodoList.Data.Dtos
{
    public class GetTodoDto : INotifyPropertyChanged
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string ShortName
        {
            get {
                if (Name.Length > 30) {
                    return Name.Substring(0, 25) + "...";
                } else {
                    return Name;
                }
            }
        }
        public bool _isCompleted { get; set; } = false;
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyChanged();
                    IsCompletedChanged?.Invoke(this, EventArgs.Empty); // Notify ViewModel
                }
            }
        }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = string.Empty;

        public event EventHandler IsCompletedChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
