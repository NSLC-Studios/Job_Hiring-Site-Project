using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaAdminInterface.Model;
using AvaloniaAdminInterface.ViewModels;
using AvaloniaAdminInterface.Views;
using JobHiringAPI.Dtos;
using System;


namespace AvaloniaAdminInterface;

public partial class LoginWindow : Window
{

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
        main.Show();

        this.Close();
    }
}
