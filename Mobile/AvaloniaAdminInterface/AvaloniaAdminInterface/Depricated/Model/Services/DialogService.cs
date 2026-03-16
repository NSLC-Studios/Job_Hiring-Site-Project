using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaAdminInterface.Model
{
    public class DialogService : DialogServiceInterface
    {
        public async Task ShowMessage(string title, string message)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(
                title,
                message,
                ButtonEnum.Ok,
                Icon.Info);

            await box.ShowAsync();
        }

        public async Task<bool> Confirm(string title, string message)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(
                title,
                message,
                ButtonEnum.YesNo,
                Icon.Question);

            var result = await box.ShowAsync();
            return result == ButtonResult.Yes;
        }
    }

}
