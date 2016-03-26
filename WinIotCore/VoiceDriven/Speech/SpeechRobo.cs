using System;
using System.Diagnostics;
using VoiceDriven.ExceptionHandlers;
using VoiceDriven.Strategies;
using Windows.Media.SpeechRecognition;
using Windows.UI.Core;

namespace VoiceDriven.Speech
{
    public class SpeechRobo
    {
        // Speech events may come in on a thread other than the UI thread, keep track of the UI thread's
        // dispatcher, so we can update the UI in a thread-safe manner.
        private CoreDispatcher dispatcher;
        
        /// <summary> 
        /// This HResult represents the scenario where a user is prompted to allow in-app speech, but  
        /// declines. This should only happen on a Phone device, where speech is enabled for the entire device, 
        /// not per-app. 
        /// </summary> 
        private static uint HResultPrivacyStatementDeclined = 0x80045509;
        private string hypothesis;

        // Grammer File
        private const string SRGS_FILE = "Grammar\\grammar.xml";
        
        // RED Led Pin
        private const int RED_LED_PIN = 5;
        // GREEN Led Pin
        private const int GREEN_LED_PIN = 6;
        // Bedroom Light Pin
        private const int BEDROOM_LIGHT_PIN = 13;
        // Tag TARGET
        private const string TAG_TARGET = "target";
        // Tag CMD
        private const string TAG_CMD = "cmd";
        // Tag Device
        private const string TAG_DEVICE = "device";


        // On State
        private const string STATE_ON = "ON";
        // Off State
        private const string STATE_OFF = "OFF";
        // LED Device
        private const string DEVICE_LED = "LED";
        // Light Device
        private const string DEVICE_LIGHT = "LIGHT";
        // Red Led
        private const string COLOR_RED = "RED";
        // Green Led
        private const string COLOR_GREEN = "GREEN";
        // Bedroom
        private const string TARGET_BEDROOM = "BEDROOM";
        // Porch
        private const string TARGET_PORCH = "PORCH";
        // Youtube
        private const string TARGET_YOUTUBE = "YOUTUBE";
        // Google
        private const string TARGET_GOOGLE = "GOOGLE";

        // Speech Recognizer
        private SpeechRecognizer recognizer;

        public async void Init()
        {
            // Keep track of the UI thread dispatcher, as speech events will come in on a separate thread.
            dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            
            // Prompt the user for permission to access the microphone. This request will only happen
            // once, it will not re-prompt if the user rejects the permission.
            bool permissionGained = await AudioCapturePermissions.RequestMicrophonePermission();

            // Initialize recognizer
            recognizer = new SpeechRecognizer();
            recognizer.UIOptions.AudiblePrompt = "Say what you want to search for...";
            recognizer.UIOptions.ExampleText = @"Ex. 'weather for London'";

            // Set timeout settings.
            // InitialSilenceTimeout = The length of time that a SpeechRecognizer detects silence 
            //   (before any recognition results have been generated) and assumes speech input is not forthcoming.
            //recognizer.Timeouts.InitialSilenceTimeout = TimeSpan.FromSeconds(6.0);

            // BabbleTimeout = The length of time that a SpeechRecognizer continues to listen to unrecognizable sounds (babble) 
            //   before it assumes speech input has ended and finalizes the recognition operation.
            //recognizer.Timeouts.BabbleTimeout = TimeSpan.FromSeconds(4.0);

            // EndSilenceTimeout = The length of time that a SpeechRecognizer detects silence 
            //   (after recognition results have been generated) and assumes speech input has ended.
            recognizer.Timeouts.EndSilenceTimeout = TimeSpan.FromMinutes(5);

            recognizer.ContinuousRecognitionSession.AutoStopSilenceTimeout = TimeSpan.FromMinutes(5);

            // Set event handlers
            recognizer.StateChanged += Recognizer_StateChanged;
            // Listen for audio input issues.
            recognizer.RecognitionQualityDegrading += Recognizer_RecognitionQualityDegrading; ;

            // Apply the dictation topic constraint to optimize for dictated freeform speech. 
            var dictationConstraint = new SpeechRecognitionTopicConstraint(SpeechRecognitionScenario.Dictation, "dictation");

            // Add a web search grammar to the recognizer.
            var webSearchGrammar = new SpeechRecognitionTopicConstraint(SpeechRecognitionScenario.WebSearch, "webSearch");


            // Load Grammer file constraint
            //string fileName = String.Format(SRGS_FILE);
            //StorageFile grammarContentFile = await Package.Current.InstalledLocation.GetFileAsync(fileName);

            //SpeechRecognitionGrammarFileConstraint grammarConstraint = new SpeechRecognitionGrammarFileConstraint(grammarContentFile);

            // Add to grammer constraint
            //recognizer.Constraints.Add(webSearchGrammar);
            recognizer.Constraints.Add(dictationConstraint);

            // Compile the dictation grammar
            SpeechRecognitionCompilationResult compilationResult = await recognizer.CompileConstraintsAsync();

            Debug.WriteLine("Grammer Compilation Status: " + compilationResult.Status.ToString());

            // If successful, display the recognition result.
            if (compilationResult.Status == SpeechRecognitionResultStatus.Success)
            {
                Debug.WriteLine("Grammer Recognition Result: " + compilationResult.ToString());
                // Handle continuous recognition events. Completed fires when various error states occur. ResultGenerated fires when 
                // some recognized phrases occur, or the garbage rule is hit. HypothesisGenerated fires during recognition, and 
                // allows us to provide incremental feedback based on what the user's currently saying. 
                recognizer.ContinuousRecognitionSession.Completed += ContinuousRecognitionSession_Completed;
                recognizer.ContinuousRecognitionSession.ResultGenerated += Recognizer_ResultGenerated;
                recognizer.HypothesisGenerated += Recognizer_HypothesisGenerated;

                try
                {
                    // Start recognition.
                    await recognizer.ContinuousRecognitionSession.StartAsync();
                    //SpeechRecognitionResult speechRecognitionResult = await recognizer.RecognizeWithUIAsync();
                }
                catch(Exception ex)
                {
                    if((uint)ex.HResult == HResultPrivacyStatementDeclined)
                    {
                        SpeechPrivacyHandler handler = new SpeechPrivacyHandler();
                        handler.Handle(ex);
                    }
                    else
                    {
                        var msgDialog = new Windows.UI.Popups.MessageDialog(ex.Message, "Exception");
                        await msgDialog.ShowAsync();
                    }
                }
            }
            else
            {
                
                Debug.WriteLine("Grammer Compilation Status: " + compilationResult.Status);
            }
        }

        private void Recognizer_RecognitionQualityDegrading(SpeechRecognizer sender, SpeechRecognitionQualityDegradingEventArgs args)
        {
            var messageDialog = new Windows.UI.Popups.MessageDialog(args.Problem.ToString(), "Audio Problem");
            messageDialog.ShowAsync();
        }
        public async void Stop()
        {
            // Stop recognizing
            await recognizer.ContinuousRecognitionSession.CancelAsync();
            //await recognizer.ContinuousRecognitionSession.StopAsync();

            recognizer.StateChanged -= Recognizer_StateChanged;
            recognizer.RecognitionQualityDegrading -= Recognizer_RecognitionQualityDegrading; ;
            recognizer.ContinuousRecognitionSession.Completed -= ContinuousRecognitionSession_Completed;
            recognizer.ContinuousRecognitionSession.ResultGenerated -= Recognizer_ResultGenerated;
            recognizer.HypothesisGenerated -= Recognizer_HypothesisGenerated;

            recognizer.Dispose();
            recognizer = null;
        }

        /// <summary>
        /// Handle events fired when a result is generated. Check for high to medium confidence, and then append the
        /// string to the end of the stringbuffer, and replace the content of the textbox with the string buffer, to
        /// remove any hypothesis text that may be present.
        /// </summary>
        /// <param name="sender">The Recognition session that generated this result</param>
        /// <param name="args">Details about the recognized speech</param>
        private void Recognizer_ResultGenerated(SpeechContinuousRecognitionSession session, 
            SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {
            Debug.WriteLine("Recognizer_ResultGenerated: " + args.Result.Confidence.ToString() + ",  " + args.Result.Text);
            // We may choose to discard content that has low confidence, as that could indicate that we're picking up
            // noise via the microphone, or someone could be talking out of earshot.
            if (args.Result.Confidence == SpeechRecognitionConfidence.Medium ||
                args.Result.Confidence == SpeechRecognitionConfidence.High)
            {

                // Do something with the recognition result.
                //Strategy strtg = CommandRecognitionFactory.Parse(args.Result.Text);
                //strtg.Execute();

                //dictatedTextBuilder.Append(args.Result.Text + " ");

                //await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                //{
                //    discardedTextBlock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                //    dictationTextBox.Text = dictatedTextBuilder.ToString();
                //    btnClearText.IsEnabled = true;
                //});
            }
            else
            {
                // In some scenarios, a developer may choose to ignore giving the user feedback in this case, if speech
                // is not the primary input mechanism for the application.
                // Here, just remove any hypothesis text by resetting it to the last known good.
                //await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                //{
                //    dictationTextBox.Text = dictatedTextBuilder.ToString();
                //    string discardedText = args.Result.Text;
                //    if (!string.IsNullOrEmpty(discardedText))
                //    {
                //        discardedText = discardedText.Length <= 25 ? discardedText : (discardedText.Substring(0, 25) + "...");

                //        discardedTextBlock.Text = "Discarded due to low/rejected Confidence: " + discardedText;
                //        discardedTextBlock.Visibility = Windows.UI.Xaml.Visibility.Visible;
                //    }
                //});
            }

        }

        // Recognizer state changed
        private void Recognizer_StateChanged(SpeechRecognizer sender, SpeechRecognizerStateChangedEventArgs args)
        {
            Debug.WriteLine("Recognizer_StateChanged: " + args.State.ToString());
            if(args.State == SpeechRecognizerState.SoundEnded)
            {
                Strategy strtg = CommandRecognitionFactory.Parse(hypothesis);
                strtg.Execute();
            }
            else if(args.State == SpeechRecognizerState.Idle)
            {
                //recognizer.ContinuousRecognitionSession.Resume();
            }
        }

        /// <summary>
        /// Handle events fired when error conditions occur, such as the microphone becoming unavailable, or if
        /// some transient issues occur.
        /// </summary>
        /// <param name="sender">The continuous recognition session</param>
        /// <param name="args">The state of the recognizer</param>
        private async void ContinuousRecognitionSession_Completed(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionCompletedEventArgs args)
        {
            Debug.WriteLine("ContinuousRecognitionSession_Completed: " + args.Status);
            if (args.Status != SpeechRecognitionResultStatus.Success)
            {
                // If TimeoutExceeded occurs, the user has been silent for too long. We can use this to 
                // cancel recognition if the user in dictation mode and walks away from their device, etc.
                // In a global-command type scenario, this timeout won't apply automatically.
                // With dictation (no grammar in place) modes, the default timeout is 20 seconds.
                if (args.Status == SpeechRecognitionResultStatus.TimeoutExceeded)
                {
                    await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        Debug.WriteLine("Automatic Time Out of Dictation");
                        //rootPage.NotifyUser("Automatic Time Out of Dictation", NotifyType.StatusMessage);
                        //DictationButtonText.Text = " Dictate";
                        //cbLanguageSelection.IsEnabled = true;
                        //dictationTextBox.Text = dictatedTextBuilder.ToString();
                        //isListening = false;
                    });
                }
                else
                {
                    await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        Debug.WriteLine("Continuous Recognition Completed: " + args.Status.ToString());
                        //rootPage.NotifyUser("Continuous Recognition Completed: " + args.Status.ToString(), NotifyType.StatusMessage);
                        //DictationButtonText.Text = " Dictate";
                        //cbLanguageSelection.IsEnabled = true;
                        //isListening = false;
                    });
                }
            }
        }

        /// <summary>
        /// While the user is speaking, update the textbox with the partial sentence of what's being said for user feedback.
        /// </summary>
        /// <param name="sender">The recognizer that has generated the hypothesis</param>
        /// <param name="args">The hypothesis formed</param>
        private async void Recognizer_HypothesisGenerated(SpeechRecognizer sender, SpeechRecognitionHypothesisGeneratedEventArgs args)
        {
            hypothesis = args.Hypothesis.Text;

            Debug.WriteLine("DETECTED>> "+hypothesis);
            // Update the textbox with the currently confirmed text, and the hypothesis combined.
            //string textboxContent = dictatedTextBuilder.ToString() + " " + hypothesis + " ...";
            //await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
            //    dictationTextBox.Text = textboxContent;
            //    btnClearText.IsEnabled = true;
            //});
        }

    }
}
