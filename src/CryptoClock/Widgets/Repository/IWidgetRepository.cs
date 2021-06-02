using System.Collections.Generic;

namespace CryptoClock.Widgets.Repository
{
    public interface IWidgetRepository
    {
        IEnumerable<Widget> GetActiveWidgets();

        IEnumerable<WidgetPreview> GetAvailableWidgets();
    }
}
