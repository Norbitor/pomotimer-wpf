using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shell;

namespace PomodoroTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum Stages
        {
            Pomodoro, ShortBreak, LongBreak
        }

        private readonly Dictionary<Stages, string> _stagesValues;
        private readonly CountdownTimer _stageTimer;
        private readonly CountupTimer _pauseTimer;
        private Stages _currentStage = Stages.Pomodoro;

        public MainWindow()
        {
            InitializeComponent();

            // Prepare dictionary with predefined times.
            _stagesValues = new Dictionary<Stages, string>
            {
                {Stages.Pomodoro, "00:25"},
                {Stages.ShortBreak, "00:05"},
                {Stages.LongBreak, "00:10"}
            };

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

        private void StageTimerOnTimeChanged(object sender, TickEventArgs tickEventArgs)
        {
            if (tickEventArgs.TimesUp)
            {
                TimerLbl.Content = "Time's Up";
                StatisticsUpdate();
                SetStartStopBtnAsStart();
                TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
            }
            else
            {
                TimerLbl.Content = tickEventArgs.TimeLeft.ToString(@"mm\:ss");
                var x = TimeSpan.Parse(_stagesValues[_currentStage]); // Current time
                // Converts the ratio of total time and past time to [0-1] range
                // and makes the progress bar moving from 0 to 1 instead of 1 to 0.
                TaskbarItemInfo.ProgressValue = 1-(tickEventArgs.TimeLeft.TotalSeconds /
                                                 x.TotalSeconds);
            }
        }

        // Increases the value stored in appropriate label according to past cycle.
        private void StatisticsUpdate()
        {
            switch (_currentStage)
            {
                case Stages.Pomodoro:
                    PomodoroCountLbl.Content = (int.Parse((string) PomodoroCountLbl.Content) + 1).ToString();
                    break;
                case Stages.LongBreak:
                    LongBreakCountLbl.Content = (int.Parse((string) LongBreakCountLbl.Content) + 1).ToString();
                    break;
                case Stages.ShortBreak:
                    ShortBreakCountLbl.Content = (int.Parse((string) ShortBreakCountLbl.Content) + 1).ToString();
                    break;
                default:
                    throw new NotSupportedException("This is not supported by application.");
            }
        }

        private void PomodoroBtn_OnClick(object sender, RoutedEventArgs e)
        {
            SetTimer(Stages.Pomodoro);
        }

        private void ShortBreakBtn_OnClick(object sender, RoutedEventArgs e)
        {
            SetTimer(Stages.ShortBreak);
        }

        private void LongBreakBtn_OnClick(object sender, RoutedEventArgs e)
        {
            SetTimer(Stages.LongBreak);
        }

        private void ResetBtn_OnClick(object sender, RoutedEventArgs e)
        {
            // Reset all timers.
            _stageTimer.Reset();
            _pauseTimer.Reset();
            // And collapse the information about passed pause time.
            PauseTimerPanel.Visibility = Visibility.Collapsed;
            SetStartStopBtnAsStart();
            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
        }

        private void StartStopBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_stageTimer.IsRunning())
                StartTimer();
            else
                StopTimer();
        }

        private void StartTimer()
        {
            _stageTimer.Start();
            if (_currentStage == Stages.Pomodoro)
                _pauseTimer.Stop();
            SetStartStopBtnAsStop();
            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
        }

        private void StopTimer()
        {
            _stageTimer.Stop();
            if (_currentStage == Stages.Pomodoro)
            {
                _pauseTimer.Start();
                if (PauseTimerPanel.Visibility != Visibility.Visible)
                    PauseTimerPanel.Visibility = Visibility.Visible;
            }
            SetStartStopBtnAsStart();
            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Paused;
        }

        private void AboutBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var about = new AboutWindow { Owner = this };
            about.ShowDialog();
        }

        private void ExpandBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (StatisticGrid.Visibility == Visibility.Collapsed)
            {
                StatisticGrid.Visibility = Visibility.Visible;
                ExpandBtn.Content = "5"; // "Up arrow" in Marlett font
            }
            else
            {
                StatisticGrid.Visibility = Visibility.Collapsed;
                ExpandBtn.Content = "6"; // "Down arrow" in Marlett font
            }
        }

        private void SetTimer(Stages stage)
        {
            _currentStage = stage;
            PauseTimerPanel.Visibility = Visibility.Collapsed;
            // Set time left using the value from dictionary.
            _stageTimer.SetTimeLeft(_stagesValues[stage], true);
            _pauseTimer.Reset();
            SetStartStopBtnAsStop();
            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
        }

        private void SetStartStopBtnAsStart()
        {
            StartStopBtn.Content = "S_tart";
            StartStopBtn.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xBC, 0xE2, 0x7F));
        }

        private void SetStartStopBtnAsStop()
        {
            StartStopBtn.Content = "S_top";
            StartStopBtn.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xED, 0xB5, 0xAE));
        }
        
    }
}
