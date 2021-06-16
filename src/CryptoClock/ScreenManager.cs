using CryptoClock.Data;
using CryptoClock.Data.Models;
using CryptoClock.Screens;
using CryptoClock.Widgets;
using CryptoClock.Widgets.Rendering;
using CryptoClock.Widgets.Repository;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoClock
{
    public class ScreenManager
    {
        private readonly ILogger<ScreenManager> log;
        private readonly IEnumerable<IDataProvider> providers;
        private readonly IWidgetRenderer renderer;
        private readonly IWidgetRepository repository;
        private readonly IScreenPrinter printer;

        public ScreenManager(
            ILogger<ScreenManager> log,
            IEnumerable<IDataProvider> providers,
            IWidgetRenderer renderer,
            IWidgetRepository repository, 
            IScreenPrinter printer)
        {
            this.log = log;
            this.providers = providers;
            this.renderer = renderer;
            this.repository = repository;
            this.printer = printer;
        }

        public async Task RefreshAsync()
        {
            var model = await LoadDataAsync();
            var configs = this.repository.GetActiveWidgets();
            var all = this.repository.GetAvailableWidgets().ToArray();
            var widgets = configs
                .Select(x => new Widget(this.repository.GetParsedWidgetById(x, model), x))
                .ToArray();

            using var image = this.renderer.Render(widgets);

            this.printer.Print(image);
        }

        private async Task<CryptoModel> LoadDataAsync()
        {
            var model = new CryptoModel();

            foreach (var provider in this.providers)
            {
                model = await provider.EnrichAsync(model);
            }

            return model;
        }
    }
}
