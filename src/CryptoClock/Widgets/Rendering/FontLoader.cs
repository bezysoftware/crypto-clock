using SkiaSharp;
using System;
using System.Linq;
using System.Reflection;

namespace CryptoClock.Widgets.Rendering
{
    public static class FontLoader
    {
        public static SKTypeface LoadTypeface(string name, SKFontStyleWeight weight)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var weightModifier = weight switch
            {
                SKFontStyleWeight.Bold => "b",
                SKFontStyleWeight.Light => "l",
                _ => ""
            };
            var file = $"{name}{weightModifier}.ttf";
            var resource = assembly.GetManifestResourceNames().First(x => x.EndsWith(file, StringComparison.InvariantCultureIgnoreCase));

            using var stream = assembly.GetManifestResourceStream(resource);

            return SKTypeface.FromStream(stream);
        }
    }
}
