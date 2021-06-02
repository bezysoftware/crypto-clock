using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CryptoClock.Configuration;
using System.Collections.Generic;
using CryptoClock.Data;
using CryptoClock.Data.Models;

namespace CryptoClock
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly IOptions<LightningConfig> options;
        private readonly IEnumerable<IDataProvider> providers;
        private readonly ScreenManager manager;

        public Worker(
            ILogger<Worker> logger, 
            IOptions<LightningConfig> options,
            IEnumerable<IDataProvider> providers,
            ScreenManager manager)
        {
            this.logger = logger;
            this.options = options;
            this.providers = providers;
            this.manager = manager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var model = await LoadDataAsync();

                await this.manager.RefreshAsync();
                
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task<CryptoModel> LoadDataAsync()
        {
            var model = new CryptoModel();

            foreach (var provider in this.providers)
            {
                await provider.EnrichAsync(model);
            }

            return model;
        }
    }
}
