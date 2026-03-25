using Avalonia.Controls;
using AvaloniaAdminInterface.Dtos;
using AvaloniaAdminInterface.Model;
using AvaloniaAdminInterface.Model.Services;
using AvaloniaAdminInterface.ViewModels;

namespace AvaloniaAdminInterface.Views;

public partial class MainWindow : Window
{
    private readonly ApiSession session;
    
    public MainWindow()
    {
        InitializeComponent();
        
        NavigationService navigation = new NavigationService(this);
        TheModel _model = new TheModel(session);
        DataContext = new MainViewModel(_model,navigation);
    }
}
