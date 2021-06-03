using CryptoClock.Widgets.Rendering.Nodes;
using System.IO;
using System.Xml.Serialization;

namespace CryptoClock.Widgets
{
    public class WidgetParser : IWidgetParser
    {
        private static XmlSerializer serializer = new XmlSerializer(typeof(WidgetNode));

        public WidgetNode LoadFromFile<T>(string id, T model)
        {
            using var stream = new FileStream($"Widgets/Definitions/{id}.xml", FileMode.Open);

            // todo populate model

            return (WidgetNode)serializer.Deserialize(stream);
        }
    }
}
