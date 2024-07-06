using AM_Proyecto.Interfaces;
using AM_Proyecto.Services;
using AM_Proyecto.ViewModels;
using AM_Proyecto.Views;
using Microsoft.Extensions.Logging;

namespace AM_Proyecto
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



#if DEBUG

            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddSingleton<IInventarioServicio, InventarioServicio>();
            builder.Services.AddSingleton<InventarioVM>();
            builder.Services.AddSingleton<IVentaServicio, VentaServicio>();
            builder.Services.AddSingleton<VentasVM>();

            return builder.Build();
        }
    }
}
