using Avalonia.Controls;
using AvaloniaAdminInterface.Model;
using AvaloniaAdminInterface.Model.Services;
using AvaloniaAdminInterface.ViewModels;
using System;

namespace AvaloniaAdminInterface.Views;
public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        this.AttachedToVisualTree += (_, __) =>
        {
            var window = TopLevel.GetTopLevel(this) as Window;

            var model = App.Model; 
            var nav = new NavigationService(window!);

            var vm = new MainViewModel(model, nav);
            DataContext = vm;

            vm.LoadUsersCommand.Execute().Subscribe();
        };
    }
}




