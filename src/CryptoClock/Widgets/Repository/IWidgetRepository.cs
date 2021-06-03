using System.Collections.Generic;

namespace CryptoClock.Widgets.Repository
{
    public interface IWidgetRepository
    {
        IEnumerable<WidgetPlacement> GetActiveWidgets();

        IEnumerable<WidgetPreview> GetAvailableWidgets();
    }
}
