using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReactiveUI;

namespace AvaloniaAdminInterface.ViewModels;

public class ViewModelBase : ReactiveObject, INotifyPropertyChanged
{
    protected ViewModelBase() { }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
    }
}
