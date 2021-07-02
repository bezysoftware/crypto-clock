using System.Xml.Serialization;

namespace CryptoClock.Widgets.Rendering.Nodes
{
    public enum ElementSize
    {
        [XmlEnum]
        Default,

        [XmlEnum("tiny")]
        Tiny,

        [XmlEnum("xsmall")]
        ExtraSmall,

        [XmlEnum("small")]
        Small,

        [XmlEnum("medium")]
        Medium,

        [XmlEnum("large")]
        Large,

        [XmlEnum("xlarge")]
        ExtraLarge,

        [XmlEnum("huge")]
        Huge
    }
}
