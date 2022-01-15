using jwala.philipshuebridge.com;
using jwala.philipshuebridge.com.responses.authorization;
using Xamarin.Forms;

namespace jwala
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void OnStart()
        {
            SelectPage();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private async void SelectPage()
        {
            var value = await Xamarin.Essentials.SecureStorage.GetAsync("bridge");
            if (value is not null)
            {
                var bridge = value.FromJson<Bridge>();
                var authJson = await Xamarin.Essentials.SecureStorage.GetAsync(bridge.Name);
                
                if (authJson is not null)
                {
                    var auth = authJson.FromJson<Success>();
                    MainPage = new MainPage(bridge, auth);
                    return;
                }
            }

            MainPage = new GettingStartedPage();
        }
    }
}
