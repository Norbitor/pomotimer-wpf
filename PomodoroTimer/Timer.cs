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
    /// Countdown timer logic with 1s resolution.
    /// </summary>
    class Timer
    {
        private readonly DispatcherTimer _timer;
        private TimeSpan _timeLeft;
        private TimeSpan _lastSetTime;
        private SoundPlayer _sound;

        public event TickEventHandler TimeLeftChange;

        public Timer()
        {
            _timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 1) };
            _timer.Tick += TimerOnTick;
            SetTimeLeft("00:25");

            Stream snd = Properties.Resources.alarm;
            _sound = new SoundPlayer(snd);
        }

        protected virtual void OnTimeLeftChange(TickEventArgs e)
        {
            TimeLeftChange?.Invoke(this, e); // Inform about event if we have listeners
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            _timeLeft -= TimeSpan.FromSeconds(1.0);
            if (_timeLeft == TimeSpan.Zero)
            {
                _timer.Stop();
                _sound.Play();
                OnTimeLeftChange(new TickEventArgs(_timeLeft, true));
            }
            else
                OnTimeLeftChange(new TickEventArgs(_timeLeft, false));
        }

        public void SetTimeLeft(string period, bool start = false)
        {
            _timer.Stop();
            _lastSetTime = _timeLeft = TimeSpan.Parse(period);
            OnTimeLeftChange(new TickEventArgs(_timeLeft, false));
            if (start) _timer.Start();
        }

        public void Start()
        {
            if(_timeLeft <= TimeSpan.Zero)
                Reset();
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void Reset()
        {
            _timer.Stop();
            _timeLeft = _lastSetTime;
            OnTimeLeftChange(new TickEventArgs(_timeLeft, false));
        }

        public bool IsRunning()
        {
            return _timer.IsEnabled;
        }
    }
}
