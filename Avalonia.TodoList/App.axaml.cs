using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.TodoList.Services;
using Avalonia.TodoList.ViewModels;
using Avalonia.TodoList.Views;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;

namespace Avalonia.TodoList;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        //try
        //{
            #region DESKTOP PLATFORMS (WINDOWS, LINUX, MAC)
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Remove Avalonia data validation to avoid duplicates with CommunityToolkit
                BindingPlugins.DataValidators.RemoveAt(0);

                #region Creates a ServiceProvider containing services from the provided IServiceCollection
                var collection = new ServiceCollection();
                collection.AddCommonServices();

                var services = collection.BuildServiceProvider();
                var vm = services.GetRequiredService<MainViewModel>();
                var mainWindow = services.GetRequiredService<MainWindow>();
                #endregion

                mainWindow.DataContext = vm;

                // Set the main window and assign the DataContext
                desktop.MainWindow = mainWindow;
            }
            #endregion

            #region FOR ANDROID AND OTHER PLATFORMS
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                #region Creates a ServiceProvider containing services from the provided IServiceCollection
                //var collection = new ServiceCollection();

                //collection.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);
                //collection.AddSingleton<TodoService>();
                //collection.AddTransient<MainViewModel>();

                //var services = collection.BuildServiceProvider();
                //var vm = services.GetRequiredService<MainViewModel>();
                
                singleViewPlatform.MainView = new MainView {
                    DataContext = new MainViewModel()
                };
                #endregion
            }
            #endregion

            // Call the base method to complete the framework initialization
            base.OnFrameworkInitializationCompleted();
        //}
        //catch (Exception ex)
        //{
        //    // Handle exceptions during initialization
        //    Console.WriteLine($"Error during initialization: {ex.Message}");
        //    // Optionally, log the error or show a user-friendly message
        //}
    }
}

/// <summary>
/// Register all the services in this extension class for IServiceCollection
/// </summary>
public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        // create a configuration builder
        //var builder = new ConfigurationBuilder()
        //    .SetBasePath(AppContext.BaseDirectory)
        //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        //// build and get the IConfiguration object
        //IConfiguration _configuration = builder.Build();

        //// Access AppSettings
        //var developerName = _configuration["AppSettings:DeveloperName"];
        //var developerAge = _configuration["AppSettings:DeveloperAge"];
        //var title = _configuration["AppSettings:Title"];

        //// Access ConnectionStrings
        //var sqlServerConnection = _configuration.GetConnectionString("SQLServerConnection");
        //var sqliteConnection = _configuration.GetConnectionString("SQLiteConnection");
        /*
            "AppSettings": {
                "DeveloperName": "Wali Qadri",
                "DeveloperAge": 37,
                "Title": "Todo app with Entity framework Example",
                "SampleList": {
                    "List1": "Hello my boi",
                    "List2": 34234324,
                    "List3": "2024-08-24T03:45:42.2157181"
                }
            }
        */

        //string? sGetDate = _configuration["AppSettings:SampleList:List3"];
        //DateTime dtObject;
        //if (sGetDate != null)
        //{
        //    dtObject = DateTime.Parse(sGetDate);
        //    var sDay = dtObject.Day.ToString();
        //    var sDayOfWeek = dtObject.DayOfWeek.ToString();
        //    var sDate = dtObject.Date.ToString();
        //    var sHour = dtObject.Hour.ToString();
        //}

        //Debug.WriteLine(sGetDate);

        //collection.AddSingleton(_configuration); // add the configuration as Singleton
                                                 // Register IMessenger as a singleton
        collection.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);
        collection.AddSingleton<TodoService>();
        collection.AddTransient<MainViewModel>();
        collection.AddTransient<MainWindow>();
    }
}

