using CryptoClock.Configuration;
using CryptoClock.Widgets.Rendering;
using CryptoClock.Widgets.Repository;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace CryptoClock
{
    public class ScreenManager
    {
        private readonly IWidgetRenderer widgetRenderer;
        private readonly IWidgetRepository widgetRepository;
        private readonly ScreenConfig screen;

        public ScreenManager(IWidgetRenderer widgetRenderer, IWidgetRepository widgetRepository, IOptions<ScreenConfig> options)
        {
            this.widgetRenderer = widgetRenderer;
            this.widgetRepository = widgetRepository;
            this.screen = options.Value;
        }

        public async Task RefreshAsync()
        {
            var widgets = this.widgetRepository.GetActiveWidgets();
            var image = this.widgetRenderer.Render(this.screen, widgets);
            var path = Path.GetFullPath("img.png");

            using (var file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                image.CopyTo(file);
            }

            Process.Start(new ProcessStartInfo { FileName = path, Verb = "open", UseShellExecute = true });
        }
    }
}
