using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceDriven.Speech;

namespace VoiceDriven.Strategies
{
    public abstract class Strategy
    {
        protected VoiceCommand cmd;
        public abstract void Execute();
    }
}
