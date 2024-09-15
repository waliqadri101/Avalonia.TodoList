using Avalonia.Controls;
using Avalonia.TodoList.Data.Dtos;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Avalonia.TodoList.UserControls
{
    public partial class EditDeletePopupControl : UserControl, INotifyPropertyChanged
    {
        #region ItemProperty
        public static readonly StyledProperty<GetTodoDto> ItemProperty =
            AvaloniaProperty.Register<EditDeletePopupControl, GetTodoDto>(nameof(Item));

        public GetTodoDto Item
        {
            get => GetValue(ItemProperty);
            set
            {
                Debug.WriteLine("Setting the main item");
                SetValue(ItemProperty, value);
            }
        }
        #endregion

        #region OTHER PROPERTIES

        private bool _isEditing = false;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                if (_isEditing != value)
                {
                    _isEditing = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        public EditDeletePopupControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private async void btnItemDelete_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (Item is GetTodoDto eachTodo)
            {
                Debug.WriteLine(eachTodo.Id.ToString());
            }
        }

        private async void btnItemEdit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (this.IsEditing == false)
            {
                // make the ui editable
                this.IsEditing = true;

                // Change the name of the button

                btnItemEdit.Content = "Save";
            }
            else
            {

                if (btnItemEdit.Content == "Save")
                {
                    await ShowMessageBox();
                }
            }
        }

        private async void btnItemCancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (this.IsEditing)
            {
                this.IsEditing = false;
                btnItemEdit.Content = "Edit";
            }
        }


        // INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private async Task ShowMessageBox()
        {
            var messageBoxStandardWindow = MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ContentTitle = "Message Box Title",
                    ContentMessage = "This is a message box.",
                    ButtonDefinitions = ButtonEnum.Ok,
                    Icon = Icon.Info,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner, // Center the message box on the screen
                });

            var result = await messageBoxStandardWindow.ShowAsPopupAsync(this);
        }

    }
}
