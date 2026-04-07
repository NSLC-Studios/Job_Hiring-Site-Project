using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaAdminInterface.Model
{
    public interface DialogServiceInterface
    {
        Task ShowMessage(string title, string message);
        Task<bool> Confirm(string title, string message);
    }
}
