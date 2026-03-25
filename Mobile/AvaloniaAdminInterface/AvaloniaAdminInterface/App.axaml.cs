using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaAdminInterface.Dtos;
using AvaloniaAdminInterface.Model;
using AvaloniaAdminInterface.Model.Services;
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
        session = new ApiSession("https://localhost:7142/");
        //AuthApi auth = new AuthApi(session);
        TheModel model = new TheModel(session);
        MainWindow mainWindow = new MainWindow();
        var nav = new NavigationService(mainWindow);
        MainViewModel viewModel = new MainViewModel(model,nav);
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new LoginWindow(model);
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new LoginWindow(model);
        }

        base.OnFrameworkInitializationCompleted();
    }
}
