using Avalonia.Controls;
using AvaloniaAdminInterface.Model;
using AvaloniaAdminInterface.ViewModels;

namespace AvaloniaAdminInterface.Views;

public partial class MainWindow : Window
{
    private readonly ApiSession session;
    public MainWindow()
    {
        InitializeComponent();
         
        TheModel _model = new TheModel(session);
        DataContext = new MainViewModel(_model);
    }
}
