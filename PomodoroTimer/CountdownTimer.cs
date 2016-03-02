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
    class CountdownTimer : Timer
    {
        private SoundPlayer _sound;

        public CountdownTimer()
        {
            SetTimeLeft("00:25");

            Stream snd = Properties.Resources.alarm;
            _sound = new SoundPlayer(snd);
        }

        protected override void TimerOnTick(object sender, EventArgs eventArgs)
        {
            CurrentTime -= TimeSpan.FromSeconds(1.0);
            if (CurrentTime == TimeSpan.Zero)
            {
                Tim.Stop();
                _sound.Play();
                OnTimeChange(new TickEventArgs(CurrentTime, true));
            }
            else
                OnTimeChange(new TickEventArgs(CurrentTime, false));
        }
    }
}
