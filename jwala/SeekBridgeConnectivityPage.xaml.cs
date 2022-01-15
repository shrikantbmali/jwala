using System;
using System.Threading.Tasks;
using jwala.philipshuebridge.com;
using jwala.philipshuebridge.com.responses.authorization;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace jwala
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SeekBridgeConnectivityPage : ContentPage
    {
        public SeekBridgeConnectivityPage(Bridge bridge)
        {
            InitializeComponent();

            BindingContext = new SeekBridgeConnectivityViewModel(bridge);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = ((SeekBridgeConnectivityViewModel)BindingContext);
            viewModel.OnAuthCompleted += ViewModel_OnAuthCompleted;
            _ = viewModel.TryConnectAsync();
        }

        private void ViewModel_OnAuthCompleted(object sender, AuthCompletedEventArgs authCompletedEventArgs)
        {
            SecureStorage.SetAsync("bridge", authCompletedEventArgs.Bridge.ToJson());
            SecureStorage.SetAsync(authCompletedEventArgs.Bridge.Name, authCompletedEventArgs.AuthSuccess.ToJson());
            App.Current.MainPage = new MainPage(authCompletedEventArgs.Bridge, authCompletedEventArgs.AuthSuccess);
        }
    }

    public class SeekBridgeConnectivityViewModel : ObservableObject
    {
        private readonly Bridge _bridge;
        private readonly BridgeAuthService _bridgeAuthService;
        private string _errorMessage;

        public event EventHandler<AuthCompletedEventArgs> OnAuthCompleted;

        public SeekBridgeConnectivityViewModel(Bridge bridge)
        {
            _bridge = bridge;
            _bridgeAuthService = new BridgeAuthService(bridge);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            private set => SetProperty(ref _errorMessage, value);
        }

        public async Task TryConnectAsync()
        {
            bool poll = true;
            while (poll)
            {
                await Task.Delay(1000);
                var auth = await _bridgeAuthService.Auth("jwala", "1");

                if (auth.Error is not null)
                {
                    await Device.InvokeOnMainThreadAsync(() => ErrorMessage = auth.Error.Description);
                }
                else if(auth.Success?.Username is not null)
                {
                    OnAuthCompleted?.Invoke(this, new AuthCompletedEventArgs(auth.Success, _bridge));
                    poll = false;
                }
            }
        }
    }

    public class AuthCompletedEventArgs : EventArgs
    {
        public Success AuthSuccess { get; }
        public Bridge Bridge { get; }

        public AuthCompletedEventArgs(Success authSuccess, Bridge bridge)
        {
            AuthSuccess = authSuccess;
            Bridge = bridge;
        }
    }
}