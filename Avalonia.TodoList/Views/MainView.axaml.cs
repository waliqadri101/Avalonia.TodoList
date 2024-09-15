using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
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
        
        // set the datacontext for the listbox
        lbMainTodo.DataContext = oDataContext;

        if (oDataContext != null)
        {
            await oDataContext.LoadData();
        }
        oDataContext = null;
    }
    private void OnDeleteButtonClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            var abd = button.Parent;
            // Traverse up the visual tree to find the Flyout
            var parent = button.Parent;
            while (parent != null && !(parent is Flyout))
            {
                parent = (parent as Control)?.Parent;
                if (parent is FlyoutPresenter flyout)
                {
                    Popup popup = ((flyout as FlyoutPresenter).Parent as Popup);

                    if (popup != null)
                    {
                        popup.Close();
                    }
                }
            }

            // Close the Flyout if found
            //if (parent is Flyout flyout)
            //{
            //    flyout.Hide();
            //}
        }
    }
}

/// <summary>
/// Use this class for the porpose of IMessage
/// </summary>
public class MessengerClass
{
    public string Message { get; set; }
}

