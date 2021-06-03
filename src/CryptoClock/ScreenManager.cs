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
        private readonly IWidgetParser parser;
        private readonly IWidgetRepository repository;
        private readonly IScreenPrinter printer;

        public ScreenManager(
            IEnumerable<IDataProvider> providers,
            IWidgetRenderer renderer, 
            IWidgetParser parser, 
            IWidgetRepository repository, 
            IScreenPrinter printer)
        {
            this.providers = providers;
            this.renderer = renderer;
            this.parser = parser;
            this.repository = repository;
            this.printer = printer;
        }

        public async Task RefreshAsync()
        {
            var model = await LoadDataAsync();
            var widgets = this.repository.GetActiveWidgets();
            var placements = widgets
                .Select(x => new Widget(this.parser.LoadFromFile(x.Id, model), x))
                .ToArray();

            using var image = this.renderer.Render(placements);

            this.printer.Print(image);
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
