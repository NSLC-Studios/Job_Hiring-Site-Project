using Avalonia.Controls;
using AvaloniaAdminInterface.Model;
using AvaloniaAdminInterface.ViewModels;

namespace AvaloniaAdminInterface.Views;

public partial class MainView : UserControl
{
    private readonly ApiSession _session;
    public MainView()
    {
        InitializeComponent();
        TheModel _model = new TheModel(_session);
        DataContext = new MainViewModel(_model);
    }
}
