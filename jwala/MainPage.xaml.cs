using System.Linq;
using jwala.philipshuebridge.com;
using jwala.philipshuebridge.com.responses.authorization;
using jwala.philipshuebridge.com.responses.resources;
using Xamarin.Forms;
using Color = Xamarin.Forms.Color;

namespace jwala
{
    public partial class MainPage : ContentPage
    {
        private BridgeCom _bridgeCom;

        public MainPage(Bridge bridge, Success success)
        {
            InitializeComponent();

            _bridgeCom = new BridgeCom(bridge, success);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            _ = await _bridgeCom.Subscribe()
                .OnEvent(message =>
                {
                    Device.InvokeOnMainThreadAsync(() =>
                    {
                        Label.Text = message.ToJson();
                        CreateNewColor(message?.Data.FirstOrDefault() ?? new Datum(), ContentPage.BackgroundColor);
                    });
                })
                .StartAsync();

        }

        private Color CreateNewColor(Datum data, Color currentColor)
        {
            if (data?.Color?.Gamut is not null)
            {
                currentColor = Color.FromRgb(data.Color.Gamut.Red.X, data.Color.Gamut.Green.X,
                    data.Color.Gamut.Green.X);
            }
            else if (data?.Color?.Xy is not null)
            {
                //currentColor = Color.FromRgb(rgb.R, rgb.G, rgb.B);
            }
            else if (data?.Dimming is not null)
            {
            }

            return currentColor;
        }
    }
}
