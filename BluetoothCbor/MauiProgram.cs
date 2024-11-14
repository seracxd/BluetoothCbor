using BluetoothCbor.Services;
using Microsoft.Extensions.Logging;

#if WINDOWS
using BluetoothCbor.Platforms.Windows;
#endif

namespace BluetoothCbor
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if WINDOWS
        builder.Services.AddSingleton<IGattServerService, GattServerService>();
#else
            // Můžete zde přidat prázdnou implementaci pro jiné platformy nebo jen vynechat
            builder.Services.AddSingleton<IGattServerService, MockGattServerService>();
#endif
            builder.Services.AddTransient<MainPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
