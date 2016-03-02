using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PomodoroTimer
{
    public delegate void TickEventHandler(object sender, TickEventArgs e);
    /// <summary>
    /// Abstract Timer logic with 1s resolution.
    /// </summary>
    abstract class Timer
    {
        protected readonly DispatcherTimer Tim;
        protected TimeSpan CurrentTime;
        protected TimeSpan LastSetTime;

        public event TickEventHandler TimeChanged;

        protected Timer()
        {
            Tim = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 1) };
            Tim.Tick += TimerOnTick;
        }

        protected virtual void OnTimeChange(TickEventArgs e)
        {
            TimeChanged?.Invoke(this, e); // Inform about event if we have listeners
        }

        protected abstract void TimerOnTick(object sender, EventArgs eventArgs);

        public void Start()
        {
            if(CurrentTime <= TimeSpan.Zero) // Prevents of operating on negative time
                Reset();
            Tim.Start();
        }

        public void Stop()
        {
            Tim.Stop();
        }

        public void Reset()
        {
            Tim.Stop();
            CurrentTime = LastSetTime;
            OnTimeChange(new TickEventArgs(CurrentTime, false));
        }

        public bool IsRunning()
        {
            return Tim.IsEnabled;
        }

        public void SetTimeLeft(string period, bool start = false)
        {
            Tim.Stop();
            LastSetTime = CurrentTime = TimeSpan.Parse(period);
            OnTimeChange(new TickEventArgs(CurrentTime, false));
            if (start) Tim.Start();
        }
    }
}
