using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace CryptoClock
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> log;
        private readonly ScreenManager manager;

        public Worker(
            ILogger<Worker> log, 
            ScreenManager manager)
        {
            this.log = log;
            this.manager = manager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await this.manager.RefreshAsync();
                } 
                catch(Exception ex)
                {
                    this.log.LogError(ex, "Exception during Worker execution");
                }

                await Task.Delay(60_000, stoppingToken);
            }
        }
    }
}
