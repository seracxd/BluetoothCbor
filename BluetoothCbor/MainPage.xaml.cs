using BluetoothCbor.DataIdentifiers;
using BluetoothCbor.Services;

namespace BluetoothCbor
{
    public partial class MainPage : ContentPage
    {
        private readonly IGattServerService _serverService;
     
        public MainPage(IGattServerService serverService)
        {
            InitializeComponent();
            _serverService = serverService;
        }

        private async void StartServerButton_Clicked(object sender, EventArgs e)
        {
            await _serverService.StartGattServerAsync();
            await DisplayAlert("Info", "GATT Server started", "OK");
        }

        private async void StopServerButton_Clicked(object sender, EventArgs e)
        {
            await _serverService.StopGattServerAsync();
            await DisplayAlert("Info", "GATT Server stopped", "OK");
        }

        private async void SendNotificationButton_Clicked(object sender, EventArgs e)
        {
            var identifiersToSend = new List<DataIdentifier> { /* Zadejte identifikátory dle potřeby */ };
            int sequenceNumber = 1; // Nastavte příslušné sekvenční číslo

            await _serverService.SendNotificationAsync(identifiersToSend, sequenceNumber);
            await DisplayAlert("Info", "Notification sent", "OK");
        }
    }
}
