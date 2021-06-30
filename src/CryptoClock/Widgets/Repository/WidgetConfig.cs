using CryptoClock.Widgets.Rendering.Nodes;
using System.Collections.Generic;

namespace CryptoClock.Widgets.Repository
{
    public record WidgetConfig(
        string Id,
        string Background,
        string Foreground,
        JustifyContent Justify,
        WidgetPlacement Placement,
        Dictionary<string, object> Data)
    {
    }
}
