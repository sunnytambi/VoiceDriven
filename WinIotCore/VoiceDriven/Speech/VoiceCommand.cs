using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceDriven.Speech
{
    public class VoiceCommand
    {
        public const string GOOGLE = "google";
        public const string YOUTUBE = "youtube";
        public const string FIRST = "first";
        public const string SECOND = "second";

        private string _CmdKey;
        private string _CmdValue;

        public string CmdKey
        {
            get
            {
                return _CmdKey;
            }
        }

        public string CmdValue
        {
            get
            {
                return _CmdValue;
            }
            set
            {
                _CmdValue = value;
            }
        }

        public VoiceCommand(string cmdkey, string cmdvalue)
        {
            _CmdKey = cmdkey;
            _CmdValue = cmdvalue;
        }
    }
}
