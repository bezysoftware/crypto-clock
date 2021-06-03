using System.Collections.Generic;

namespace CryptoClock.Widgets.Repository
{
    public class WidgetRepository : IWidgetRepository
    {
        public IEnumerable<WidgetPlacement> GetActiveWidgets()
        {
            return new[]
            {
                new WidgetPlacement("Weather", 0, 0, 4, 4),
                new WidgetPlacement("Lightning", 4, 2, 6, 4)
            };
        }

        public IEnumerable<WidgetPreview> GetAvailableWidgets()
        {
            return new[]
            {
                new WidgetPreview("Weather", "Description"),
                new WidgetPreview("Lightning", "Description")
            };
        }
    }
}
