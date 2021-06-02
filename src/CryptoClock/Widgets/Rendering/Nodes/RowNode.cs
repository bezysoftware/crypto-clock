using System.Collections.Generic;
using System.Xml.Serialization;

namespace CryptoClock.Widgets.Rendering.Nodes
{
    [XmlType("row")]
    public class RowNode : ElementNodeBase
    {
        [XmlElement("column")]
        public List<ColumnNode> Columns { get; set; }

        public override TResult Render<TContext, TResult>(TContext context, IWidgetRenderer<TContext, TResult> renderer)
        {
            return renderer.Render(context, this);
        }
    }
}
