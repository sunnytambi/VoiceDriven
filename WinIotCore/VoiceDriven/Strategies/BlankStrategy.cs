using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceDriven.Strategies
{
    public class BlankStrategy : Strategy
    {
        public override void Execute()
        {
            Debug.WriteLine("Reached blank strategy. Executes nothing");
        }
    }
}
