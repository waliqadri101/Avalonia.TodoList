using Avalonia.Controls;

namespace Avalonia.TodoList.Views;

public partial class MainWindow : Window
{
    //Constructors
    public MainWindow()
    {
        InitializeComponent();
    }

    //Events
    private void Window_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        //PositionWindow();
    }


    //// methods
    //private void PositionWindow()
    //{
    //    // Get the primary screen's working area (usable screen size)
        
    //    if (Screens.Primary != null)    // had to do this for those stupid null warning
    //    {
    //        var screen = Screens.Primary.WorkingArea;

    //        // Get the actual width of the window
    //        double applicationWidth = this.Bounds.Width;

    //        // Calculate the position for the top-right corner, adjusted for DPI scaling
    //        double applicationLeft = (screen.Width - (int)this.DesiredSize.Width);

    //        // Set the window position to the top-right corner
    //        this.Position = new PixelPoint((int)applicationLeft - 100, (int)(screen.Y));
    //    }
    //}
    //private void msg(string sMessage)
    //{
    //    System.Diagnostics.Debug.WriteLine(sMessage);
    //}
}