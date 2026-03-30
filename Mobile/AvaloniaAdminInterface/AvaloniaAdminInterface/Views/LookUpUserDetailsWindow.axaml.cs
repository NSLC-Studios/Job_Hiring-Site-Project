using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Reactive.Linq;

namespace AvaloniaAdminInterface;

public partial class LookUpUserDetailsWindow : Window
{

    public LookUpUserDetailsWindow()
    {
        InitializeComponent();

        this.AttachedToVisualTree += (_, __) =>
        {
            if (DataContext is LookUpUserDetailsViewModel vm)
                //vm.LoadCommand.Execute().Subscribe();
                vm.LoadCommand.Execute().Subscribe(null);
        };
    }
}