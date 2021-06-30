using System.Xml.Serialization;

namespace CryptoClock.Widgets.Rendering.Nodes
{
    public enum JustifyContent
    {
        [XmlIgnore]
        Unset,

        [XmlEnum("start")]
        Start,

        [XmlEnum("center")]
        Center,

        [XmlEnum("end")]
        End,

        [XmlEnum("spread")]
        Spread
    }
}
