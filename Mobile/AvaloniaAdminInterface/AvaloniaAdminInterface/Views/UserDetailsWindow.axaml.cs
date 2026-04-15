using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Reactive.Linq;

namespace AvaloniaAdminInterface;

public partial class UserDetailsViewModel : Window
{

    public UserDetailsViewModel()
    {
        InitializeComponent();
        //yes it passes an empty event
        this.Opened += (_, __) =>
        {
            if (DataContext is UserDetailsViewModel vm)
            {
                //vm.LoadCommand.Execute().Subscribe();
                vm.LoadAsync();//pain
            }
        };
    }
}

