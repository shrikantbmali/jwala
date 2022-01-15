using System;
using System.Timers;
using Xamarin.Forms;

namespace jwala
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var timer = new Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = TimeSpan.FromMilliseconds(100).Milliseconds;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Device.InvokeOnMainThreadAsync(() =>
            {
                Label.Text = e.SignalTime.ToLongTimeString();
                
                return ContentPage.BackgroundColor = new Xamarin.Forms.Color(e.SignalTime.Second,
                        e.SignalTime.Millisecond, e.SignalTime.Ticks);
            });
        }
    }
}
