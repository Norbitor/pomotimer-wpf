using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroTimer
{
    class CountupTimer : Timer
    {
        public CountupTimer()
        {
            SetTimeLeft("00:00");  
        }

        protected override void TimerOnTick(object sender, EventArgs eventArgs)
        {
            CurrentTime += TimeSpan.FromSeconds(1.0);
            OnTimeChange(new TickEventArgs(CurrentTime, false));
        }
    }
}
