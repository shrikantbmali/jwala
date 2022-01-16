using System;
using System.Linq;
using ColorHelper;
using jwala.philipshuebridge.com;
using jwala.philipshuebridge.com.responses.authorization;
using jwala.philipshuebridge.com.responses.resources;
using Xamarin.Forms;
using Color = Xamarin.Forms.Color;

namespace jwala;

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

        var resources = await _bridgeCom.GetResourceAsync("light");

        _ = await _bridgeCom.SubscribeAsync()
            .OnEvent(message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Label.Text = message.ToJson();

                    ContentPage.BackgroundColor = CreateNewColor(message?.Data.FirstOrDefault() ?? new Datum(), ContentPage.BackgroundColor);
                });
            })
            .StartAsync();

    }


    private Color CreateNewColor(Datum data, Color currentColor)
    {
        if (data?.Color?.Xy is not null)
        {
            var xyToRgb = xyToRGB(data.Color.Xy.X, data.Color.Xy.Y, 255);
            return Color.FromRgb(xyToRgb.R, xyToRgb.G, xyToRgb.B);
        }
        else if (data?.Dimming is not null)
        {
            return Color.FromHsla(currentColor.Hue, currentColor.Saturation, currentColor.Luminosity, data.Dimming.Brightness / 100);
        }



        return currentColor;
    }

    private (byte R, byte G, byte B) xyToRGB(double x, double y, double bri)
    {
        var z = 1.0 - x - y;
        var Y = bri / 255.0; // Brightness of lamp
        var X = (Y / y) * x;
        var Z = (Y / y) * z;
        var r = X * 1.612 - Y * 0.203 - Z * 0.302;
        var g = -X * 0.509 + Y * 1.412 + Z * 0.066;
        var b = X * 0.026 - Y * 0.072 + Z * 0.962;

        r = r <= 0.0031308 ? 12.92 * r : (1.0 + 0.055) * Math.Pow(r, (1.0 / 2.4)) - 0.055;
        g = g <= 0.0031308 ? 12.92 * g : (1.0 + 0.055) * Math.Pow(g, (1.0 / 2.4)) - 0.055;
        b = b <= 0.0031308 ? 12.92 * b : (1.0 + 0.055) * Math.Pow(b, (1.0 / 2.4)) - 0.055;
        
        var maxValue = Math.Max(Math.Max(r,g), b);
        
        r /= maxValue;
        g /= maxValue;
        b /= maxValue;
        r *= 255;
        
        if (r < 0)
        {
            r = 255;
        }

        g *= 255;
        if (g < 0)
        {
            g = 255;
        }

        b *= 255;
        if (b < 0)
        {
            b = 255;
        }

        return ((byte r, byte y, byte z))(r, g, b);
    }
}