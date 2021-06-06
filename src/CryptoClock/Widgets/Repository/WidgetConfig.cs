using System.Collections.Generic;

namespace CryptoClock.Widgets.Repository
{
    public record WidgetConfig(
        string Id,
        string Background,
        string Foreground,
        WidgetPlacement Placement,
        Dictionary<string, object> Data)
    {
    }
}
