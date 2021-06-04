using CryptoClock.Data;
using CryptoClock.Data.Models;
using CryptoClock.Screens;
using CryptoClock.Widgets;
using CryptoClock.Widgets.Rendering;
using CryptoClock.Widgets.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoClock
{
    public class ScreenManager
    {
        private readonly IEnumerable<IDataProvider> providers;
        private readonly IWidgetRenderer renderer;
        private readonly IWidgetRepository repository;
        private readonly IScreenPrinter printer;

        public ScreenManager(
            IEnumerable<IDataProvider> providers,
            IWidgetRenderer renderer,
            IWidgetRepository repository, 
            IScreenPrinter printer)
        {
            this.providers = providers;
            this.renderer = renderer;
            this.repository = repository;
            this.printer = printer;
        }

        public async Task RefreshAsync()
        {
            var model = await LoadDataAsync();
            var widgets = this.repository.GetActiveWidgets();
            var all = this.repository.GetAvailableWidgets().ToArray();
            var placements = widgets
                .Select(x => new Widget(this.repository.GetParsedWidgetById(x.Id, model), x))
                .ToArray();

            using var image = this.renderer.Render(placements);

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
