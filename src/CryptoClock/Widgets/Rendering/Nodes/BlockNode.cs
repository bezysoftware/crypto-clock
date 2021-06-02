using System.Collections.Generic;
using System.Xml.Serialization;

namespace CryptoClock.Widgets.Rendering.Nodes
{
    public class BlockNode : ElementNodeBase, IVerticalElementsNode
    {
        [XmlElement("row", typeof(RowNode))]
        [XmlElement("text", typeof(TextNode))]
        [XmlElement("image", typeof(ImageNode))]
        [XmlElement("line", typeof(LineNode))]
        public List<ElementNodeBase> Elements { get; set; }

        public override TResult Render<TContext, TResult>(TContext context, IWidgetRenderer<TContext, TResult> renderer)
        {
            return renderer.Render(context, this);
        }
    }
}
