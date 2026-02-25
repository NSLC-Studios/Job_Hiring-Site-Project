using Avalonia.Controls;
using AvaloniaAdminInterface.ViewModels;

namespace AvaloniaAdminInterface.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
        
    }
}
