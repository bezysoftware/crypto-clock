using CryptoClock.Widgets.Rendering.Nodes;
using System.Collections.Generic;

namespace CryptoClock.Widgets.Repository
{
    public interface IWidgetRepository
    {
        IEnumerable<WidgetConfig> GetActiveWidgets();

        IEnumerable<WidgetPreview> GetAvailableWidgets();

        WidgetNode GetParsedWidgetById<T>(WidgetConfig config, T model);
    }
}
