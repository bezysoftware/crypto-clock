using CryptoClock.Widgets.Rendering.Nodes;

namespace CryptoClock.Widgets
{
    public interface IWidgetParser
    {
        WidgetNode LoadFromFile<T>(string id, T model);
    }
}
