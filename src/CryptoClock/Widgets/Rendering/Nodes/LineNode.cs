using System.Xml.Serialization;

namespace CryptoClock.Widgets.Rendering.Nodes
{
    [XmlType("line")]
    public class LineNode : ElementNodeBase
    {
        [XmlAttribute("foreground")]
        public string Foreground { get; set; }

        [XmlAttribute("size")]
        public ElementSize Size { get; set; }

        [XmlAttribute("divider")]
        public bool IsDivider { get; set; }

        public override TResult Render<TContext, TResult>(TContext context, IWidgetRenderer<TContext, TResult> renderer)
        {
            return renderer.Render(context, this);
        }
    }
}
