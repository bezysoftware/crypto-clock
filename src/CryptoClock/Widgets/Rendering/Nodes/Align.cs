using System.Xml.Serialization;

namespace CryptoClock.Widgets.Rendering.Nodes
{
    public enum Align
    {
        [XmlEnum("start")]
        Start,

        [XmlEnum("center")]
        Center,

        [XmlEnum("end")]
        End
    }
}
