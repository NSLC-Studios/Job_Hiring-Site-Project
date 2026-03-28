using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaAdminInterface.Model;
using AvaloniaAdminInterface.Model.Services;
using AvaloniaAdminInterface.ViewModels;
using AvaloniaAdminInterface.Views;

namespace AvaloniaAdminInterface;

public partial class App : Application
{
    public static ApiSession Session { get; private set; } = null!;
    public static TheModel Model { get; private set; } = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Session = new ApiSession("https://localhost:7142/");
        Model = new TheModel(Session);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // 2. Show login window FIRST
            desktop.MainWindow = new LoginWindow(Model);
        }

        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new LoginWindow(Model);
        }

        base.OnFrameworkInitializationCompleted();
    }
}
