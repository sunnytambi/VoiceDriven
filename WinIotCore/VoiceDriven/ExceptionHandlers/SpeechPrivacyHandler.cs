using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceDriven.ExceptionHandlers
{
    public class SpeechPrivacyHandler : IExceptionHandler
    {
        public async void Handle(Exception ex)
        {
            string ErrMsg = "The privacy statement was declined. " +
                "Go to Settings->Privacy->Speech, inking and typing, and ensure you " +
                "have viewed the privacy policy, and 'Get To Know You' is enabled.";
            var messageDialog = new Windows.UI.Popups.MessageDialog(ErrMsg, "Speech Privacy Exception");
            
            await messageDialog.ShowAsync();
            
            // Open the privacy/speech, inking, and typing settings page.
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-accounts"));
        }
    }
}
