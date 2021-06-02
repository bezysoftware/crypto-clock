using CryptoClock.Widgets.Rendering.Nodes;
using System.IO;
using System.Xml.Serialization;

namespace CryptoClock.Widgets
{
    public static class WidgetParser
    {
        private static XmlSerializer serializer = new XmlSerializer(typeof(WidgetNode));

        public static WidgetNode LoadFromFile(string id)
        {
            using var stream = new FileStream($"Widgets/Definitions/{id}.xml", FileMode.Open);

            return (WidgetNode)serializer.Deserialize(stream);
        }
    }
}
