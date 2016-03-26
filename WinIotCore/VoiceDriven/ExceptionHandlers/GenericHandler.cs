using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceDriven.ExceptionHandlers
{
    public class GenericHandler : IExceptionHandler
    {
        public async void Handle(Exception ex)
        {
            var messageDialog = new Windows.UI.Popups.MessageDialog(ex.Message, "Exception");
            await messageDialog.ShowAsync();
        }
    }
}
