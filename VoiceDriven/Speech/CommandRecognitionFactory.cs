using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceDriven.Strategies;

namespace VoiceDriven.Speech
{
    public class CommandRecognitionFactory
    {
        public static Strategy Parse(string speechRecognitionOutput)
        {
            Strategy strtg = new BlankStrategy();
            if (!string.IsNullOrEmpty(speechRecognitionOutput))
            {
                char[] delimiters = new char[] { ' ' };
                string[] splittedOutput = speechRecognitionOutput.Split(delimiters, 2);

                string key = splittedOutput[0].ToLower();
                VoiceCommand cmd = new VoiceCommand(key, null);

                switch (key)
                {
                    case VoiceCommand.YOUTUBE:
                        if(splittedOutput.Count() >= 2)
                        {
                            string value = splittedOutput[1].ToLower();
                            cmd.CmdValue = value;
                        }
                        else
                        {
                            cmd.CmdValue = string.Empty;
                        }
                        strtg = new YoutubePlayStrategy(cmd);
                        break;
                    case VoiceCommand.FIRST:
                        strtg = new SelectionStrategy(cmd);
                        break;
                    case VoiceCommand.SECOND:
                        strtg = new SelectionStrategy(cmd);
                        break;
                    default:
                        strtg = new BlankStrategy();
                        break;
                }
            }

            return strtg;
        }
    }
}
