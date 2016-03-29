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
    /// <summary>
    /// Timer which is counting remaining time.
    /// </summary>
    class CountdownTimer : Timer
    {
        private readonly SoundPlayer _sound;

        public CountdownTimer()
        {
            SetTimeLeft("00:25");

            // Create a Sound Player to beep after the programmed period ends.
            Stream snd = Properties.Resources.alarm;
            _sound = new SoundPlayer(snd);
        }

        protected override void TimerOnTick(object sender, EventArgs eventArgs)
        {
            CurrentTime -= TimeSpan.FromSeconds(1.0);
            // If the countdown period end, stop the timer and play the sound.
            if (CurrentTime == TimeSpan.Zero)
            {
                Tim.Stop();
                _sound.Play();
                OnTimeChange(new TickEventArgs(CurrentTime, true));
            }
            else
            {
                OnTimeChange(new TickEventArgs(CurrentTime, false));
            }
        }
    }
}
