using Avalonia.Controls;
using AvaloniaAdminInterface.Model;
using AvaloniaAdminInterface.Model.Services;
using AvaloniaAdminInterface.ViewModels;
using System;

namespace AvaloniaAdminInterface.Views;

public partial class MainView : UserControl
{
    private readonly ApiSession _session;
    private readonly Window mainWindow;
    //private readonly MainViewModel _viewModel;

    public MainView()
    {
        InitializeComponent();
        
        /*
        var session = _session;
        var model = new TheModel(session);

        var window = TopLevel.GetTopLevel(this) as Window;
        var nav = new NavigationService(window);

        DataContext = new MainViewModel(model, nav);*/

        this.AttachedToVisualTree += (_, __) =>
        {
            var window = TopLevel.GetTopLevel(this) as Window;

            var model = App.Model;
            var nav = new NavigationService(window);

            var vm = new MainViewModel(model, nav);
            DataContext = vm;

            vm.LoadUsersCommand.Execute().Subscribe();
        };
    }

}




