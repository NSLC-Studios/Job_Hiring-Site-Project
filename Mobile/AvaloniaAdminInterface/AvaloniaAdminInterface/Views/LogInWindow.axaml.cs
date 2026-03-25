using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaAdminInterface.Model;
using AvaloniaAdminInterface.Model.Services;
using AvaloniaAdminInterface.ViewModels;
using AvaloniaAdminInterface.Views;
using JobHiringAPI.Dtos;
using System;


namespace AvaloniaAdminInterface;


public partial class LoginWindow : Window
{
    private readonly TheModel _model;

    public LoginWindow(TheModel model)
    {
        InitializeComponent();
        _model = model;

        var vm = new LogInViewModel(model);
        vm.LoginSucceeded += OnLoginSucceeded;
        DataContext = vm;
    }

    private void OnLoginSucceeded(UserLoginDto dto)
    {
        var main = new MainWindow();
        var nav = new NavigationService(main);

        main.DataContext = new MainViewModel(_model, nav);

        main.Show();
        this.Close();
    }
}

/*


    public LoginWindow(TheModel model)
    {
        InitializeComponent();

        var vm = new LogInViewModel(model);
        DataContext = vm;

        // Subscribe to login success 
        vm.LoginSucceeded += OnLoginSucceeded;
    }

    private void OnLoginSucceeded(UserLoginDto dto)
    {
        var main = new MainWindow();

        var session = new ApiSession("https://localhost:7142/");
        var model = new TheModel(session);
        var nav = new NavigationService(main);

        main.DataContext = new MainViewModel(model, nav);

        main.Show();
        this.Close();
    }
    */
    /*var main = new MainWindow();
     main.Show();

     this.Close();
}*/

