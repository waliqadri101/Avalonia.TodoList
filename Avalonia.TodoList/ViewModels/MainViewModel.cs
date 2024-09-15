using Avalonia.Controls;
using Avalonia.TodoList.Data.Dtos;
using Avalonia.TodoList.Data.Entities;
using Avalonia.TodoList.Services;
using Avalonia.TodoList.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace Avalonia.TodoList.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    #region FIELDS AND PROPERTIES
    public string SampleTextValueToTestBindings { get; set; } = "hi there";

    private TodoService? _todoService;
    private readonly IMessenger _messenger;

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private ObservableCollection<GetTodoDto> _todoList;

    [ObservableProperty]
    private ObservableCollection<GetTodoDto> _todoListCompleted;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(OkayButtonCommand))]
    private string _newTodoItem;
    #endregion

    // constructors
    // empty constructor is needed by the axaml file for some reason.
    public MainViewModel()
    {
        TodoList = new ObservableCollection<GetTodoDto>();
        TodoListCompleted = new ObservableCollection<GetTodoDto>();
        Title = "Todo Application";
    }
    public MainViewModel(TodoService todoService, IMessenger messenger) : this()
    {
        // intialize services
        _todoService = todoService;
        _messenger = messenger;
    }

    /// <summary>
    /// This method was making the ui disappeared when the Api call was being made.
    /// So Had to call this from the code behind file, on the Window(UserControl) loaded event
    /// </summary>
    /// <returns></returns>
    public async Task LoadData()
    {
        if (!Design.IsDesignMode)
        {
            // if the application is actually running then do the API call to fetch Live data. else get dummy data

            // reset the list
            TodoList = new ObservableCollection<GetTodoDto>();
            TodoListCompleted = new ObservableCollection<GetTodoDto>();

            // get the data from the api
            List<Todo> getAllTodos = await _todoService.GetAllTodosAsync();
            
            // compile two list one for complete and one for uncompleted todo and attach the checkbox event.
            foreach (Todo eachTodo in getAllTodos)
            {
                // create a GetTodoDto item with the proper values
                GetTodoDto oGetTodoDto = new GetTodoDto()
                {
                    Id = eachTodo.Id,
                    Name = eachTodo.Name,
                    IsCompleted = eachTodo.IsCompleted,
                    CreatedOn = eachTodo.CreatedOn,
                    CreatedBy = eachTodo.CreatedBy
                };

                // attach the checkbox click event
                oGetTodoDto.IsCompletedChanged += TodoItem_IsCompletedChanged;

                if (eachTodo.IsCompleted)
                {
                    TodoListCompleted.Add(oGetTodoDto);
                }
                else
                {
                    TodoList.Add(oGetTodoDto);
                }
            }

            // Serialize to JSON
            // string json = JsonSerializer.Serialize(TodoList);
        }
        else
        {
            // ONLY FOR XAML DESIGNER : Get some Dummy data and Deserialize into TodoList

            string sTodoListUnCompletedJson = "[{\"Id\":1,\"Name\":\"string very long data to display in an item to see if it truncates\",\"IsCompleted\":false,\"CreatedOn\":\"2024-08-24T03:38:29.1799151\",\"CreatedBy\":\"Sample User\"},{\"Id\":6,\"Name\":\"Creating a new todo list\",\"IsCompleted\":false,\"CreatedOn\":\"2024-08-24T20:00:26.9997771\",\"CreatedBy\":\"Sample User\"}]";
            string sTodoListCompleteJson = "[{\"Id\":3,\"Name\":\"New todo list item.\",\"IsCompleted\":true,\"CreatedOn\":\"2024-08-24T03:45:42.2157181\",\"CreatedBy\":\"Sample User\"},{\"Id\":5,\"Name\":\"Another item test\",\"IsCompleted\":true,\"CreatedOn\":\"2024-08-24T04:05:08.0375123\",\"CreatedBy\":\"Sample User\"}]";
            TodoList = JsonSerializer.Deserialize<ObservableCollection<GetTodoDto>>(sTodoListUnCompletedJson);
            TodoListCompleted = JsonSerializer.Deserialize<ObservableCollection<GetTodoDto>>(sTodoListCompleteJson);
        }
    }

    // Todo checkbox value change event.
    private async void TodoItem_IsCompletedChanged(object sender, EventArgs e)
    {
        if (sender is GetTodoDto todoItem)
        {
            Debug.WriteLine("CheckBox Clicked on " + todoItem.Name);
            Debug.WriteLine("Calling update api on " + todoItem.Id);
            // call the api to update the database
            if (await _todoService.UpdateTodoAsync(todoItem))
            {
                await LoadData();
            }
        }
    }

    #region RELAY COMMANDS
    [RelayCommand]
    private void CancelFlyOut(object? param)
    {
        // send a message to the code behind so it can close that flyout.
        _messenger.Send(new MessengerClass() { Message = "CloseFlyOut" });
    }

    /// <summary>
    /// THIS IS JUST AN EXAMPLE WILL SEND A MESSAGE AND THE CODE BEHIDN WILL CHANGE THE WIDTH OF THE USERCONTROL TO 400
    /// </summary>
    /// <param name="param"></param>
    [RelayCommand(CanExecute = nameof(CanExecuteOkayButton))]
    private async void OkayButton(object? param)
    {
        if (NewTodoItem != null && NewTodoItem.Length > 0)
        {
            Todo? newTodo = await _todoService!.CreateNewTodoAsync(new CreateTodoDto() {
                Name = NewTodoItem,
                IsCompleted = false,
                CreatedBy = "Avalonia User"
            });

            if (newTodo != null && newTodo.Id > 0)
            {
                await LoadData();
            }
            else
            {
                throw new Exception("Unable to create a new todo item.");
            }
        }
    }
    private bool CanExecuteOkayButton(object? param)
    {
        if (NewTodoItem != null && NewTodoItem.Length > 0)
        {
            return true; // or false based on some condition
        }
        else {
            return false;
        }
    }


    [RelayCommand]
    private void ItemOptionClick(object? param)
    {
        if (param != null)
        {
            Debug.WriteLine("CheckBox Clicked on " + param!.ToString());
        }
    }

    [RelayCommand]
    private void ItemOptionDeleteClick(object? param)
    {
        if (param != null)
        {
            Debug.WriteLine("Delete button clicked for " + param!.ToString());
        }
    }
    [RelayCommand]
    private void ItemOptionEditClick(object? param)
    {
        if (param != null)
        {
            Debug.WriteLine("Edit button clicked for " + param!.ToString());
        }
    }

    #endregion
}

