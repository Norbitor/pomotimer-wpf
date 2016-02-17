using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroTimer
{
    /// <summary>
    /// EventArguments for Timer class
    /// </summary>
    public class TickEventArgs : EventArgs
    {
        public TimeSpan TimeLeft { get; private set; }
        public bool TimesUp { get; private set; }

        public TickEventArgs(TimeSpan timeLeft, bool timesUp)
        {
            TimeLeft = timeLeft;
            TimesUp = timesUp;
        }
    }
}
