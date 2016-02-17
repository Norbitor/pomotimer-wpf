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
        private Timer _timer;

        public MainWindow()
        {
            InitializeComponent();
            _timer = new Timer();
            _timer.TimeLeftChange += TimerOnTimeLeftChange;
        }

        private void TimerOnTimeLeftChange(object sender, TickEventArgs eventArgs)
        {
            timerLbl.Content = eventArgs.TimesUp ? "Time's Up" : eventArgs.TimeLeft.ToString(@"mm\:ss");
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
            _timer.SetTimeLeft("00:25", true);
        }

        private void ShortBreakBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _timer.SetTimeLeft("00:00:05", true);
        }

        private void LongBreakBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _timer.SetTimeLeft("00:10", true);
        }

        private void ResetBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _timer.Reset();
        }
    }
}
