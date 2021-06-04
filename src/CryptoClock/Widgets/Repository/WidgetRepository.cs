using CryptoClock.Widgets.Rendering.Nodes;
using Scriban;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace CryptoClock.Widgets.Repository
{
    public class WidgetRepository : IWidgetRepository
    {
        private static XmlSerializer BaseSerializer = new XmlSerializer(typeof(WidgetNodeBase));
        private static XmlSerializer Serializer = new XmlSerializer(typeof(WidgetNode));

        private static string WidgetDefinitionsLocation = "Widgets/Definitions";

        public IEnumerable<WidgetPlacement> GetActiveWidgets()
        {
            return new[]
            {
                new WidgetPlacement("Weather", 0, 0, 4, 4),
                new WidgetPlacement("Lightning", 4, 2, 6, 4)
            };
        }

        public IEnumerable<WidgetPreview> GetAvailableWidgets()
        {
            var files = Directory.GetFiles(WidgetDefinitionsLocation, "*.xml");

            foreach (var file in files)
            {
                var id = Path.GetFileNameWithoutExtension(file);

                using (var stream = new FileStream(file, FileMode.Open))
                {
                    var node = (WidgetNodeBase)BaseSerializer.Deserialize(stream);
                    yield return new WidgetPreview(id, node.Description);
                }
            }
        }

        public WidgetNode GetParsedWidgetById<T>(string id, T model)
        {
            var file = Path.Combine(WidgetDefinitionsLocation, $"{id}.xml");
            var content = File.ReadAllText(file);
            var template = Template.Parse(content).Render(model, x => x.Name);

            using var reader = new StringReader(template);

            return (WidgetNode)Serializer.Deserialize(reader);
        }
    }
}
