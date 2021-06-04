using System.Collections.Generic;
using System.Xml.Serialization;

namespace CryptoClock.Widgets.Rendering.Nodes
{
    [XmlRoot("widget")]
    public class WidgetNodeBase
    {
        [XmlAttribute("description")]
        public string Description { get; set; }
    }

    [XmlRoot("widget")]
    public class WidgetNode : WidgetNodeBase
    {
        [XmlElement("binding")]
        public List<BindingNode> Bindings { get; set; }
    }
}
