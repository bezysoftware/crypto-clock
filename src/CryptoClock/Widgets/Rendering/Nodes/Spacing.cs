using System.Xml.Serialization;

namespace CryptoClock.Widgets.Rendering.Nodes
{
    public enum Spacing
    {
        [XmlEnum("regular")]
        Regular,

        [XmlEnum("none")]
        None
    }
}
