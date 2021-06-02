using System.Collections.Generic;

namespace CryptoClock.Widgets.Rendering.Nodes
{
    public interface IVerticalElementsNode
    {
        List<ElementNodeBase> Elements { get; }
    }
}
