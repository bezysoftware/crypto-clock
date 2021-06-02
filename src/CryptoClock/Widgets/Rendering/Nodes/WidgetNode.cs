using System.Collections.Generic;
using System.Xml.Serialization;

namespace CryptoClock.Widgets.Rendering.Nodes
{
    [XmlType("widget")]
    public class WidgetNode
    {
        [XmlElement("binding")]
        public List<BindingNode> Bindings { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }
    }
}
