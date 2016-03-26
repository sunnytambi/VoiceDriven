using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceDriven.Speech;
using VoiceDriven.Subscribers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VoiceDriven.Strategies
{
    public class YoutubePlayStrategy : Strategy
    {
        public YoutubePlayStrategy(VoiceCommand vcmd)
        {
            cmd = vcmd;
        }

        public async override void Execute()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, ()=> 
            {
                Frame rootFrame = Window.Current.Content as Frame;
                BrowserPage currentPage = rootFrame.Content as BrowserPage;
                cmd.CmdValue = "https://www.youtube.com/results?search_query=" + cmd.CmdValue;
                currentPage.Navigate(cmd);
            });
        }
    }
}
