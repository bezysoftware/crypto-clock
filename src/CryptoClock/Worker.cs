using Grpc.Net.Client;
using CryptoClock.Services.Lnd;
using System.Net.Http;
using Grpc.Core;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using static CryptoClock.Services.Lnd.Lightning;
using Microsoft.Extensions.Options;
using CryptoClock.Configuration;
using System.IO;
using System;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Generic;
using CryptoClock.DataProviders;
using CryptoClock.Models;

namespace CryptoClock
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly IOptions<LightningConfig> options;
        private readonly IEnumerable<IDataProvider> providers;

        public Worker(
            ILogger<Worker> logger, 
            IOptions<LightningConfig> options,
            IEnumerable<IDataProvider> providers)
        {
            this.logger = logger;
            this.options = options;
            this.providers = providers;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var model = await LoadDataAsync();
                
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task<CryptoModel> LoadDataAsync()
        {
            var model = new CryptoModel();

            foreach (var provider in this.providers)
            {
                await provider.ProvideForAsync(model);
            }

            return model;
        }
    }
}
