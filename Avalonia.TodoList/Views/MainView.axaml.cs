using Avalonia.Controls;
using Avalonia.TodoList.Services;
using Avalonia.TodoList.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace Avalonia.TodoList.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        // Register to receive the message
        WeakReferenceMessenger.Default.Register<MessengerClass>(this, (r, m) =>
        {
            if (m.Message == "CloseFlyOut")
            {
                // Access the button's Flyout and close it
                var flyout = this.btnAddNewWithFlyout.Flyout;

                if (flyout != null && flyout.IsOpen)
                {
                    flyout.Hide();
                }
            }
            else if (m.Message == "ChangeTheWidthOfTheUserControl")
            {
                this.Width = 400;
            }
        });

    }

    private async void UserControl_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainViewModel? oDataContext = (MainViewModel?)this.DataContext;
        if (oDataContext != null)
        {
            await oDataContext.LoadData();
        }
        oDataContext = null;
    }
}

/// <summary>
/// Use this class for the porpose of IMessage
/// </summary>
public class MessengerClass
{
    public string Message { get; set; }
}

