using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using VoiceDriven.Speech;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechRecognition;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VoiceDriven
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            SpeechRobo robo = new SpeechRobo();
            robo.Init();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //DataTransferManager _dataTransferManager = DataTransferManager.GetForCurrentView();   
            //  _dataTransferManager.DataRequested += OnDataRequested;   
            //  _navigationHelper.OnNavigatedTo(e);   
            //  await MainViewModel.LoadDataAsync();   
              //if (e.NavigationMode == NavigationMode.New)   
              //{
              //      await InstallVoiceCommandsAsync();
              //}   
        }

        //private async Task InstallVoiceCommandsAsync()
        //{
        //    Uri uriVoiceCommands = new Uri("ms-appx:///VoiceCommands.xml");
        //    StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uriVoiceCommands);
        //    await VoiceCommandManager.InstallCommandSetsFromStorageFileAsync(file);
        //}
    }
}
