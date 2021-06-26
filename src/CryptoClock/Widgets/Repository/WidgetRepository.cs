using CryptoClock.Widgets.Rendering.Nodes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Scriban;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace CryptoClock.Widgets.Repository
{
    public class WidgetRepository : IWidgetRepository
    {
        private static XmlSerializer BaseSerializer = new XmlSerializer(typeof(WidgetNodeBase));
        private static XmlSerializer Serializer = new XmlSerializer(typeof(WidgetNode));

        private static string WidgetDefinitionsLocation = "Assets/Widgets";
        private static string WidgetConfigsFileName = $"{Consts.DataFolderName}/Widgets.json";
        private static string WidgetConfigsDefaultFileName = "defaultwidgets.json";
        
        private readonly ILogger<WidgetRepository> log;

        public WidgetRepository(ILogger<WidgetRepository> log)
        {
            this.log = log;
        }

        public IEnumerable<WidgetConfig> GetActiveWidgets()
        {
            var appdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var file = Path.Combine(appdata, WidgetConfigsFileName);

            try
            {
                return ReadConfigFile(file);
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException)
            {
                // copy default config to appdata folder
                this.log.LogDebug($"Settings file '{file}' doesn't exist yet, copying over default.");
                Directory.CreateDirectory(Path.GetDirectoryName(file));
                File.Copy(WidgetConfigsDefaultFileName, file);
                return ReadConfigFile(file);
            }
            catch(Exception ex)
            {
                this.log.LogError(ex, $"Couldn't read widget configs from {file}");
                return Enumerable.Empty<WidgetConfig>();
            }
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

        public WidgetNode GetParsedWidgetById<T>(WidgetConfig config, T model)
        {
            var file = Path.Combine(WidgetDefinitionsLocation, $"{config.Id}.xml");
            var content = File.ReadAllText(file);
            var template = Template.Parse(content).Render(new { Model = model, Config = config }, x => x.Name);

            using var reader = new StringReader(template);

            return (WidgetNode)Serializer.Deserialize(reader);
        }

        private static IEnumerable<WidgetConfig> ReadConfigFile(string file)
        {
            var data = File.ReadAllText(file);

            return JsonConvert.DeserializeObject<WidgetConfig[]>(data) ?? Enumerable.Empty<WidgetConfig>();
        }
    }
}
