using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VoiceDriven.Speech;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VoiceDriven.Subscribers
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BrowserPage : Page
    {
        private SpeechRobo robo = new SpeechRobo();
        private string CurrentState;

        public BrowserPage()
        {
            Loaded += BrowserPage_Loaded;
            Unloaded += BrowserPage_Unloaded;
            this.InitializeComponent();

            UrlBar.GotFocus += UrlBar_GotFocus; ;
            UrlBar.LostFocus += UrlBar_LostFocus;

            GoBtn.Focus(FocusState.Keyboard);

            robo.Init();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            VoiceCommand cmd = e.Parameter as VoiceCommand;
            Navigate(cmd);
        }

        /// <summary> 
        /// Upon leaving, clean up the speech recognizer. Ensure we aren't still listening, and disable the event  
        /// handlers to prevent leaks. 
        /// </summary> 
        /// <param name="e">Unused navigation parameters.</param> 
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        { 
            if (robo != null) 
            {
                robo.Stop();
            }
        }

        private void BrowserPage_Loaded(object sender, RoutedEventArgs e)
        {
            AddText();
        }

        // Release resources, stop recognizer, release pins, etc...
        private void BrowserPage_Unloaded(object sender, object args)
        {
        }

        private void UrlBar_LostFocus(object sender, RoutedEventArgs e)
        {
            AddText();
        }

        private void UrlBar_GotFocus(object sender, RoutedEventArgs e)
        {
            RemoveText();
        }

        public void Navigate(VoiceCommand cmd)
        {
            if (null != cmd)
            {
                CurrentState = cmd.CmdKey;
                Navigate(cmd.CmdValue);
            }
        }

        public async void Select(VoiceCommand cmd)
        {
            if(CurrentState.Equals(VoiceCommand.YOUTUBE, StringComparison.CurrentCultureIgnoreCase))
            {
                if(cmd.CmdKey.Equals(VoiceCommand.FIRST))
                {
                    string script = "document.getElementsByClassName('item-section')[0].children(0).getElementsByTagName('h3')[0].children(0).click();";
                    await wv.InvokeScriptAsync("eval", new string[] { script});
                }
                else if(cmd.CmdKey.Equals(VoiceCommand.SECOND))
                {
                    string script = "document.getElementsByClassName('item-section')[0].children(1).getElementsByTagName('h3')[0].children(0).click();";
                    await wv.InvokeScriptAsync("eval", new string[] { script });
                }
            }
        }

        public void Navigate(string url)
        {
            wv.Source = new Uri(url);
        }

        private void GoBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate("http://www.google.com");
        }

        private void UrlBar_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Navigate(UrlBar.Text);
            }
        }

        private void wv_ContentLoading(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            LoadingTxt.Visibility = Visibility.Visible;
            //Progress.Visibility = Visibility.Visible;
        }

        private void wv_DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            LoadingTxt.Visibility = Visibility.Collapsed;
            //Progress.Visibility = Visibility.Collapsed;
            if (args.Uri != null)
            {
                UrlBar.Text = args.Uri.ToString();
            }
        }

        private void RemoveText()
        {
            UrlBar.Text = "";
        }

        private void AddText()
        {
            if (string.IsNullOrEmpty(UrlBar.Text))
            {
                UrlBar.Text = "<<URL>>";
            }
        }
    }
}
