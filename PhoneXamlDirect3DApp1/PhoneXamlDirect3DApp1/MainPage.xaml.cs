using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;
using Windows.Phone.Speech.Synthesis;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace PhoneXamlDirect3DApp1
{
    public partial class MainPage : PhoneApplicationPage
    {

        private DispatcherTimer _timer = null;



        // Constructor
        public MainPage()
        {

            InitializeComponent();
        }
        
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds( 500 );
            _timer.Tick += TimerOnTick;

            timers.AddLast(new EnergyGameTimer(25, "Chrono"));
            timers.AddLast(new TimeGameTimer(17, "Probe"));
            timers.AddLast(new TimeGameTimer(35, "Warp"));
            timers.AddLast(new TimeGameTimer(30, "Map. Minerals."));
            synth = new SpeechSynthesizer();

            bw = new System.ComponentModel.BackgroundWorker();
            bw.DoWork += TimerOnTick;
            bw.RunWorkerAsync();

            PhoneApplicationService phoneAppService = PhoneApplicationService.Current;
            phoneAppService.UserIdleDetectionMode = IdleDetectionMode.Disabled;

//            _timer.Start();
        }


        private LinkedList<GameTimer> timers = new LinkedList<GameTimer>();
        private SpeechSynthesizer synth;
        private BackgroundWorker bw;
        private IdleDetectionMode prevIdleMode;

        private async void TimerOnTick(object sender, EventArgs eventArgs)
        {
            try
            {
                while (true)
                {
                    foreach (var timer in timers)
                    {
                        try
                        {
                            if (timer.LastNotify + timer.Recharge < DateTime.Now)
                            {
                                await synth.SpeakTextAsync(timer.Name);
                                timer.LastNotify = DateTime.Now;
                            }
                        }
                        // ReSharper disable once EmptyGeneralCatchClause
                        catch (Exception)
                        {
                        }
                    }
                }
                Thread.Sleep(1000);
            }
            finally
            {
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            var frame = ((PhoneApplicationFrame)Application.Current.RootVisual);
            frame.Obscured += delegate(object s, ObscuredEventArgs args)
            {
                if (bw != null)
                {
                    bw.CancelAsync();
                }
            };
            frame.Unobscured += delegate(object s, EventArgs args)
            {
                if (bw != null)
                {
                    bw.RunWorkerAsync();
                }
            };

        }
    }
}