using System.Xml.Serialization;

namespace CryptoClock.Widgets.Rendering.Nodes
{
    public abstract class ElementNodeBase
    {
        [XmlAttribute("align")]
        public Align Align { get; set; }

        public abstract TResult Render<TContext, TResult>(TContext context, IWidgetRenderer<TContext, TResult> renderer);
    }
}
