using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceDriven.Speech;

namespace VoiceDriven.Strategies
{
    public class VideoPlayStrategy : Strategy
    {
        public VideoPlayStrategy(VoiceCommand vcmd)
        {
            cmd = vcmd;
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
