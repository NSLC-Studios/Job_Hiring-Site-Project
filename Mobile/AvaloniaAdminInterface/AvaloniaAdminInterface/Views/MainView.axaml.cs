using Avalonia.Controls;
using AvaloniaAdminInterface.Dtos;
using AvaloniaAdminInterface.Model;
using AvaloniaAdminInterface.Model.Services;
using AvaloniaAdminInterface.ViewModels;

namespace AvaloniaAdminInterface.Views;

public partial class MainView : UserControl
{
    private readonly ApiSession _session;
    private readonly Window mainWindow;
    //private readonly MainViewModel _viewModel;
    public MainView()
    {
        InitializeComponent();

        var session = _session;              
        var model = new TheModel(session);
        var nav = new NavigationService(mainWindow);           

        DataContext = new MainViewModel(model, nav);
    }
}
