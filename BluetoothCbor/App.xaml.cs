using Microsoft.Extensions.DependencyInjection;

namespace BluetoothCbor
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            MainPage = serviceProvider.GetService<MainPage>();
        }
    }
}
