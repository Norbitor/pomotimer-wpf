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
            _timer.Reset();
        }

        private void TimerOnTimeLeftChange(object sender, TickEventArgs eventArgs)
        {
            if (eventArgs.TimesUp)
            {
                TimerLbl.Content = "Time's Up";
                SetStartStopBtnAsStart();
            }
            else
            {
                TimerLbl.Content = eventArgs.TimeLeft.ToString(@"mm\:ss");
            }
            
        }

        private void PomodoroBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _timer.SetTimeLeft("00:25", true);
            SetStartStopBtnAsStop();
        }

        private void ShortBreakBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _timer.SetTimeLeft("00:05", true);
            SetStartStopBtnAsStop();
        }

        private void LongBreakBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _timer.SetTimeLeft("00:10", true);
            SetStartStopBtnAsStop();
        }

        private void ResetBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _timer.Reset();
            SetStartStopBtnAsStart();
        }

        private void StartStopBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (_timer.IsRunning())
            {
                _timer.Stop();
                SetStartStopBtnAsStart();
            }
            else
            {
                _timer.Start();
                SetStartStopBtnAsStop();
            }
        }

        private void SetStartStopBtnAsStart()
        {
            StartStopBtn.Content = "Start";
            StartStopBtn.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xBC, 0xE2, 0x7F));
        }

        private void SetStartStopBtnAsStop()
        {
            StartStopBtn.Content = "Stop";
            StartStopBtn.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xED, 0xB5, 0xAE));
        }
    }
}
