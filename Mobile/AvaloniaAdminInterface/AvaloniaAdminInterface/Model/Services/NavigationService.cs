using Avalonia.Controls;
using global::AvaloniaAdminInterface.ViewModels;

namespace AvaloniaAdminInterface.Model.Services
{
    public class NavigationService : INavigationService
    {
        private Window _mainWindow;

        public NavigationService(Window mainWindow)
        {
            _mainWindow = mainWindow;
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
            _mainWindow.DataContext = viewModel;
        }

        public void CloseCurrent()
        {
            _mainWindow.Close();
        }
    }

}
