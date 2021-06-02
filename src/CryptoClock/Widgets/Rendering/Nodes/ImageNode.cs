using System.Xml.Serialization;

namespace CryptoClock.Widgets.Rendering.Nodes
{
    [XmlType("image")]
    public class ImageNode : ElementNodeBase
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        public override TResult Render<TContext, TResult>(TContext context, IWidgetRenderer<TContext, TResult> renderer)
        {
            return renderer.Render(context, this);
        }
    }
}
