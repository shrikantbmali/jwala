using System;
using System.Collections.Generic;
using System.Windows.Input;
using jwala.philipshuebridge.com;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace jwala
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GettingStartedPage : ContentPage
    {
        public GettingStartedPage()
        {
            InitializeComponent();
            BindingContext = new GettingStartedViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((GettingStartedViewModel)BindingContext).DiscoverCommand?.Execute(null);
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Application.Current.MainPage = new SeekBridgeConnectivityPage(e.SelectedItem as Bridge);
        }
    }

    public class GettingStartedViewModel : ObservableObject
    {
        public ICommand DiscoverCommand { get; }

        private List<Bridge> _bridges;

        public List<Bridge> Bridges
        {
            get => _bridges;
            set => SetProperty(ref _bridges, value);

        }

        public ICommand BridgeSelectedCommand { get; }

        public GettingStartedViewModel()
        {
            DiscoverCommand = new Command(async () =>
            {
                var bridgeDiscoverer = new BridgeDiscoverer();

                var bridges =new List<Bridge>();

                await foreach (var bridge in bridgeDiscoverer.DiscoverBridges())
                {
                    bridges.Add(bridge);
                }

                _ = Device.InvokeOnMainThreadAsync(() => Bridges = bridges);
            });

            BridgeSelectedCommand = new Command<Bridge>(bridge =>
            {
                Console.WriteLine(bridge);
            });
        }
    }
}