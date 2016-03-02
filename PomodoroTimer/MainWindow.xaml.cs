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
        private CountdownTimer _stageTimer;
        private CountupTimer _pauseTimer;

        public MainWindow()
        {
            InitializeComponent();
            _stageTimer = new CountdownTimer();
            _stageTimer.TimeChanged += StageTimerOnTimeChanged;
            _stageTimer.Reset();

            _pauseTimer = new CountupTimer();
            _pauseTimer.TimeChanged += PauseTimerOnTimeChanged;
            _pauseTimer.Reset();
        }

        private void PauseTimerOnTimeChanged(object sender, TickEventArgs tickEventArgs)
        {
            PauseTimerLbl.Content = tickEventArgs.TimeLeft.ToString(@"mm\:ss");
        }

        private void StageTimerOnTimeChanged(object sender, TickEventArgs eventArgs)
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
            _stageTimer.SetTimeLeft("00:25", true);
            _pauseTimer.Reset();
            SetStartStopBtnAsStop();
        }

        private void ShortBreakBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _stageTimer.SetTimeLeft("00:05", true);
            _pauseTimer.Reset();
            SetStartStopBtnAsStop();
        }

        private void LongBreakBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _stageTimer.SetTimeLeft("00:10", true);
            _pauseTimer.Reset();
            SetStartStopBtnAsStop();
        }

        private void ResetBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _stageTimer.Reset();
            _pauseTimer.Reset();
            SetStartStopBtnAsStart();
        }

        private void StartStopBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (_stageTimer.IsRunning())
            {
                _stageTimer.Stop();
                _pauseTimer.Start();
                SetStartStopBtnAsStart();
            }
            else
            {
                _stageTimer.Start();
                _pauseTimer.Stop();
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

        private void AboutBtn_OnClick(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow {Owner = this};
            about.ShowDialog();
        }
    }
}
