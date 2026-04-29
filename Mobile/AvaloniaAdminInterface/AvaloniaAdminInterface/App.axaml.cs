using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaAdminInterface.Model;
using AvaloniaAdminInterface.Model.Services;
using AvaloniaAdminInterface.ViewModels;
using AvaloniaAdminInterface.Views;
using ReactiveUI;

namespace AvaloniaAdminInterface;

public partial class App : Application
{
    public static ApiSession Session { get; private set; } = null!;
    public static TheModel Model { get; private set; } = null!;

    private MainWindow? _desktopMain;
    private MainView? _androidMain;

    private LoginWindow? _loginWindow;



    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
    private void WireLogin(LoginWindow login, Window main)
    {
        login.LoginSucceeded += user =>
        {
            main.DataContext = new MainViewModel(Model, new NavigationService(main));
            main.Show();
            login.Close();
        };
    }
    private async void OpenUserDetails(int userId)
    {
        var detailsVm = new UserDetailsViewModel(Model, userId);
        await detailsVm.LoadAsync();   //my pain

        var detailsWindow = new UserDetailsWindow
        {
            DataContext = detailsVm
        };

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            detailsWindow.Show(desktop.MainWindow);
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime android)
        {
            android.MainView = detailsWindow;
        }
    }


    public override void OnFrameworkInitializationCompleted()
    {
        Session = new ApiSession("https://localhost:7142/");
        Model = new TheModel(Session);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var login = new LoginWindow(Model);

            login.LoginSucceeded += user =>
            {
                var main = new MainWindow();
                var vm = new MainViewModel(Model, new NavigationService(main));
                /*
                vm.WhenAnyValue(x => x.SelectedUser)
                .Subscribe(user =>
                {
                     if (user != null)
                        vm.ExpandUser(user);
                });*/
                


                main.DataContext = vm;

                // Load initial data
                vm.LoadUsersCommand.Execute().Subscribe();
                vm.LoadCompaniesCommand.Execute().Subscribe();

                main.Show();
                login.Close();
            };

            desktop.MainWindow = login;
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime android)
        {
            // Android uses a single combined view
            var androidView = new AndroidView();

            // AndroidView needs a LoginViewModel first
            var loginVm = new LogInViewModel(Model);

            loginVm.LoginSucceeded += user =>
            {
                // After login, switch to MainViewModel
                var vm = new MainViewModel(Model, new NavigationService(androidView));
                androidView.DataContext = vm;

                vm.LoadUsersCommand.Execute().Subscribe();
                //vm.LoadCompaniesCommand.Execute().Subscribe();
                
            };

            androidView.DataContext = loginVm;
            android.MainView = androidView;
        }

        base.OnFrameworkInitializationCompleted();
    }



}

