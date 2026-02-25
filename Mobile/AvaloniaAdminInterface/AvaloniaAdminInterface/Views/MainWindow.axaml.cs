using Avalonia.Controls;
using AvaloniaAdminInterface.ViewModels;

namespace AvaloniaAdminInterface.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}
