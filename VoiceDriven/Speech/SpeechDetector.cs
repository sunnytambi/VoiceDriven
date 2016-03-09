using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceDriven.ExceptionHandlers;

namespace VoiceDriven.Speech
{
    public class SpeechDetector
    {
        private static uint HResultPrivacyStatementDeclined = 0x80045509;

        public static bool IsSpeechAvailble()
        {
            //IExceptionHandler handler;
            //// Handle the speech privacy policy error.
            //if ((uint)exception.HResult == HResultPrivacyStatementDeclined)
            //{
            //    handler = new SpeechPrivacyHandler();
            //}
            //else
            //{
            //    handler = new GenericHandler();
            //}
            //handler.Handle(exception);

            return false;
        }
    }
}
