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
        private enum Stages
        {
            Pomodoro, ShortBreak, LongBreak
        }

        private Dictionary<Stages, string> _stagesValues;
         
        private CountdownTimer _stageTimer;
        private CountupTimer _pauseTimer;

        private Stages _currentStage = Stages.Pomodoro;

        public MainWindow()
        {
            InitializeComponent();
            _stageTimer = new CountdownTimer();
            _stageTimer.TimeChanged += StageTimerOnTimeChanged;
            _stageTimer.Reset();

            _pauseTimer = new CountupTimer();
            _pauseTimer.TimeChanged += PauseTimerOnTimeChanged;
            _pauseTimer.Reset();

            _stagesValues = new Dictionary<Stages, string>
            {
                {Stages.Pomodoro, "00:25"},
                {Stages.ShortBreak, "00:05"},
                {Stages.LongBreak, "00:10"}
            };
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
            }
            else
                TimerLbl.Content = tickEventArgs.TimeLeft.ToString(@"mm\:ss");
        }

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

        private void SetTimer(Stages stage)
        {
            _currentStage = stage;
            PauseTimerPanel.Visibility = Visibility.Collapsed;
            _stageTimer.SetTimeLeft(_stagesValues[stage], true);
            _pauseTimer.Reset();
            SetStartStopBtnAsStop();
        }

        private void ResetBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _stageTimer.Reset();
            _pauseTimer.Reset();
            PauseTimerPanel.Visibility = Visibility.Collapsed;
            SetStartStopBtnAsStart();
        }

        private void StartStopBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (_stageTimer.IsRunning())
            {
                _stageTimer.Stop();
                if (_currentStage == Stages.Pomodoro)
                {
                    _pauseTimer.Start();
                    if(PauseTimerPanel.Visibility != Visibility.Visible)
                        PauseTimerPanel.Visibility = Visibility.Visible;
                }
                SetStartStopBtnAsStart();
            }
            else
            {
                _stageTimer.Start();
                if (_currentStage == Stages.Pomodoro)
                    _pauseTimer.Stop();
                SetStartStopBtnAsStop();
            }
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

        private void AboutBtn_OnClick(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow {Owner = this};
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
    }
}
