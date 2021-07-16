using SkiaSharp;
using System.IO;

namespace CryptoClock.Widgets.Rendering
{
    public static class FontLoader
    {
        private static string WidgetDefinitionsLocation = "Assets/Fonts/{0}.ttf";
        
        public static SKTypeface LoadTypeface(string name, SKFontStyleWeight weight)
        {
            var weightModifier = weight switch
            {
                SKFontStyleWeight.Bold => "b",
                SKFontStyleWeight.Light => "l",
                _ => ""
            };
            
            var file = string.Format(WidgetDefinitionsLocation, $"{name.ToLower()}{weightModifier}");
            
            using var stream = File.OpenRead(file);

            return SKTypeface.FromStream(stream);
        }
    }
}
