using CryptoClock.Configuration;
using CryptoClock.DataProviders;
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
                    services.Configure<LightningConfig>(context.Configuration.GetSection("LN"));
                    services.Configure<WeatherConfig>(context.Configuration.GetSection("Weather"));
                    services.Configure<PriceConfig>(context.Configuration.GetSection("Price"));
                    services.AddHostedService<Worker>();
                    services.AddHttpClient();
                    services.AddSingleton<IDataProvider, PriceDataProvider>();
                    services.AddSingleton<IDataProvider, DateTimeDataProvider>();
                    // services.AddSingleton<IDataProvider, LightningDataProvider>();
                    services.AddSingleton<IDataProvider, WeatherDataProvider>();
                });
    }
}
