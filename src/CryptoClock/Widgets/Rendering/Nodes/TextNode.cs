using System.Xml.Serialization;

namespace CryptoClock.Widgets.Rendering.Nodes
{
    [XmlType("text")]
    public class TextNode : ElementNodeBase, IFontNode
    {
        [XmlText]
        public string Text { get; set; }

        [XmlAttribute("size")]
        public FontSize FontSize { get; set; }

        [XmlAttribute("weight")]
        public string FontWeight { get; set; }

        [XmlAttribute("font")]
        public string Font { get; set; }

        public override TResult Render<TContext, TResult>(TContext context, IWidgetRenderer<TContext, TResult> renderer)
        {
            return renderer.Render(context, this);
        }
    }
}
