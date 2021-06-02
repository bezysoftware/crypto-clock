using System.Collections.Generic;
using System.Xml.Serialization;

namespace CryptoClock.Widgets.Rendering.Nodes
{
    [XmlType("column")]
    public class ColumnNode : ElementNodeBase, IVerticalElementsNode
    {
        [XmlAttribute("width")]
        public string WidthString
        {
            get => Width?.OriginalString;
            set => Width = ColumnWidth.Parse(value);
        }

        [XmlElement("text", typeof(TextNode))]
        [XmlElement("image", typeof(ImageNode))]
        [XmlElement("line", typeof(LineNode))]
        [XmlElement("block", typeof(BlockNode))]
        [XmlElement("row", typeof(RowNode))]
        public List<ElementNodeBase> Elements { get; set; }

        [XmlIgnore]
        public ColumnWidth Width { get; set; } = ColumnWidth.Default;

        public override TResult Render<TContext, TResult>(TContext context, IWidgetRenderer<TContext, TResult> renderer)
        {
            return renderer.Render(context, this);
        }
    }
}
