using Avalonia.Controls;
using global::AvaloniaAdminInterface.ViewModels;

namespace AvaloniaAdminInterface.Model.Services
{
    public class NavigationService : INavigationService
    {
        private readonly object _host;

        public NavigationService(object host)
        {
            _host = host;
        }

        public void OpenWindow(ViewModelBase viewModel)
        {
            var window = new Window
            {
                DataContext = viewModel,
                Width = 800,
                Height = 600
            };

            window.Show();
        }

        public void NavigateTo(ViewModelBase viewModel)
        {
            switch (_host)
            {
                case Window w:
                    w.DataContext = viewModel;
                    break;

                case UserControl uc:
                    uc.DataContext = viewModel;
                    break;
            }
        }

        public void CloseCurrent()
        {
            if (_host is Window w)
                w.Close();
        }
    }

}
