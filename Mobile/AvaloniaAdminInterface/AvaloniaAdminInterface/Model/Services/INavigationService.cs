using AvaloniaAdminInterface.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaAdminInterface.Model.Services
{
    public interface INavigationService
    {
        void OpenWindow(ViewModelBase viewModel);
        void NavigateTo(ViewModelBase viewModel);
        void CloseCurrent();
    }

}
