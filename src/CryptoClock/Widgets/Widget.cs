using CryptoClock.Widgets.Rendering.Nodes;
using CryptoClock.Widgets.Repository;

namespace CryptoClock.Widgets
{
    public record Widget(WidgetNode Node, WidgetConfig Config)
    {
        public WidgetPlacement Placement => Config.Placement;
    }
}
