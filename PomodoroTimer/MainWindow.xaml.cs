using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PomodoroTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer _timer;
        private TimeSpan _timeLeft;
        private TimeSpan _lastSetTime;
        public MainWindow()
        {
            InitializeComponent();
            _timer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 1)};
            _timer.Tick += TimerOnTick;
            SetTimeLeft("00:25:00");
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            _timeLeft -= TimeSpan.FromSeconds(1.0);
            timerLbl.Content = _timeLeft.ToString(@"mm\:ss");
            if (_timeLeft == TimeSpan.Zero)
            {
                _timer.Stop();
                timerLbl.Content = "Time's Up!";

                Stream snd = Properties.Resources.alarm;
                SoundPlayer simpleSound = new SoundPlayer(snd);
                simpleSound.Play();
            }
        }

        private void StartBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _timer.Start();
        }

        private void StopBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
        }

        private void PomodoroBtn_OnClick(object sender, RoutedEventArgs e)
        {
            SetTimeLeft("00:25:00", true);
        }

        private void ShortBreakBtn_OnClick(object sender, RoutedEventArgs e)
        {
            SetTimeLeft("00:05:00", true);
        }

        private void LongBreakBtn_OnClick(object sender, RoutedEventArgs e)
        {
            SetTimeLeft("00:10:00", true);
        }

        private void ResetBtn_OnClick(object sender, RoutedEventArgs e)
        {
            SetTimeLeft(_lastSetTime.ToString());
        }

        private void SetTimeLeft(string period, bool start = false)
        {
            _timer.Stop();
            _lastSetTime = _timeLeft = TimeSpan.Parse(period);
            timerLbl.Content = _timeLeft.ToString(@"mm\:ss");
            if (start) _timer.Start();
        }
    }
}
