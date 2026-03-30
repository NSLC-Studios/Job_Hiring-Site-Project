using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Reactive.Linq;

namespace AvaloniaAdminInterface;

public partial class LookUpUserDetailsWindow : Window
{

    public LookUpUserDetailsWindow()
    {
        InitializeComponent();

        this.Opened += (_, __) =>
        {
            if (DataContext is LookUpUserDetailsViewModel vm)
            {
                //vm.LoadCommand.Execute().Subscribe();
                vm.LoadAsync();
            }
        };
    }
}

