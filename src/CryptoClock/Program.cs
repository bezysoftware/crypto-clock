using CryptoClock.Configuration;
using CryptoClock.Data;
using CryptoClock.Data.Debug;
using CryptoClock.Screens;
using CryptoClock.Widgets;
using CryptoClock.Widgets.Rendering;
using CryptoClock.Widgets.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CryptoClock
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.Environment.SetEnvironmentVariable("GRPC_SSL_CIPHER_SUITES", "HIGH+ECDSA");
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.Configure<BitcoinConfig>(context.Configuration.GetSection("Bitcoin"));
                    services.Configure<LightningConfig>(context.Configuration.GetSection("LN"));
                    services.Configure<WeatherConfig>(context.Configuration.GetSection("Weather"));
                    services.Configure<PriceConfig>(context.Configuration.GetSection("Price"));
                    services.Configure<RenderConfig>(context.Configuration.GetSection("Rendering"));
                    services.Configure<ScreenConfig>(context.Configuration.GetSection("Screen"));

                    services.AddSingleton<IDataProvider, PriceDataProvider>();
                    services.AddSingleton<IDataProvider, DateTimeDataProvider>();
                    services.AddSingleton<IDataProvider, WeatherDataProvider>();
                    services.AddSingleton<IWidgetRenderer, WidgetRenderer>();
                    services.AddSingleton<IWidgetRepository, WidgetRepository>();
                    services.AddSingleton<ScreenManager>();

                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        services.AddSingleton<IDataProvider, DebugLightningDataProvider>();
                        services.AddSingleton<IDataProvider, DebugBitcoinDataProvider>();
                        services.AddSingleton<IScreenPrinter, DebugScreenPrinter>();
                    }
                    else
                    {
                        services.AddSingleton<IDataProvider, LightningDataProvider>();
                        services.AddSingleton<IDataProvider, BitcoinDataProvider>();
                        services.AddSingleton<IScreenPrinter, ScreenPrinter>();
                    }

                    services.AddHttpClient();
                    services.AddMemoryCache();
                    services.AddHostedService<Worker>();
                });
    }
}
