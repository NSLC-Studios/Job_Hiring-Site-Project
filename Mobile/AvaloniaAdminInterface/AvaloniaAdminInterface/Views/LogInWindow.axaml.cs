using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaAdminInterface.ViewModels;
using AvaloniaAdminInterface.Views;


namespace AvaloniaAdminInterface;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();

        var vm = new LogInViewModel(new AuthService());
        vm.LoginSucceeded += OnLoginSucceeded;

        DataContext = vm;
    }

    private void OnLoginSucceeded()
    {
        var main = new MainWindow();
        main.Show();

        this.Close();
    }
}
