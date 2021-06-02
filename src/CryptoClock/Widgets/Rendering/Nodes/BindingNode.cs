using System.Collections.Generic;
using System.Xml.Serialization;

namespace CryptoClock.Widgets.Rendering.Nodes
{
    [XmlType("binding")]
    public class BindingNode : ElementNodeBase, IVerticalElementsNode, IFontNode
    {
        [XmlAttribute("sizes")]
        public string Sizes { get; set; }

        [XmlAttribute("justify")]
        public JustifyContent Justify { get; set; }

        [XmlAttribute("background")]
        public string Background { get; set; }

        [XmlAttribute("foreground")]
        public string Foreground { get; set; }

        [XmlAttribute("font")]
        public string Font { get; set; }

        [XmlAttribute("font-size")]
        public FontSize FontSize { get; set; }

        [XmlAttribute("font-weight")]
        public string FontWeight { get; set; }

        [XmlElement("text", typeof(TextNode))]
        [XmlElement("image", typeof(ImageNode))]
        [XmlElement("row", typeof(RowNode))]
        [XmlElement("line", typeof(LineNode))]
        [XmlElement("block", typeof(BlockNode))]
        public List<ElementNodeBase> Elements { get; set; }

        public override TResult Render<TContext, TResult>(TContext context, IWidgetRenderer<TContext, TResult> renderer)
        {
            return renderer.Render(context, this);
        }
    }
}
