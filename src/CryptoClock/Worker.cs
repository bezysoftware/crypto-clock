using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CryptoClock
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly ScreenManager manager;

        public Worker(
            ILogger<Worker> logger, 
            ScreenManager manager)
        {
            this.logger = logger;
            this.manager = manager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await this.manager.RefreshAsync();                
                await Task.Delay(60_000, stoppingToken);
            }
        }
    }
}
