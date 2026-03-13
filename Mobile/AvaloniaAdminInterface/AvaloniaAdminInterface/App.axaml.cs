using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaAdminInterface.Model;
using AvaloniaAdminInterface.ViewModels;
using AvaloniaAdminInterface.Views;

namespace AvaloniaAdminInterface;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    static ApiSession session;
    public override void OnFrameworkInitializationCompleted()
    {
        session = new ApiSession("https://localhost:????/");
        AuthApi auth = new AuthApi(session);
        TheModel model = new TheModel(session);
 //ViewModel a modelt kapja meg meg
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {/*
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
            */
            desktop.MainWindow = new LoginWindow();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {/*
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };*/
            singleViewPlatform.MainView = new LoginWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
