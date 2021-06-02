using CryptoClock.Configuration;
using SkiaSharp;
using System.Collections.Generic;
using System.IO;

namespace CryptoClock.Widgets.Rendering
{
    public static class RenderingExtensions
    {
        public static Stream Render(this IWidgetRenderer renderer, ScreenConfig screen, IEnumerable<Widget> widgets)
        {
            var width = screen.Width / screen.Cols;
            var height = screen.Height / screen.Rows;
            var surface = SKSurface.Create(new SKImageInfo(screen.Width, screen.Height));

            foreach (var w in widgets)
            {
                var node = WidgetParser.LoadFromFile(w.Id);
                using (var result = renderer.Render(screen, node, w.Cols * width, w.Rows * height, w.Cols, w.Rows))
                {
                    surface.Canvas.DrawImage(SKImage.FromEncodedData(result), new SKPoint(w.Left * width, w.Top * height));
                }
            }

            return surface.Snapshot().Encode(SKEncodedImageFormat.Png, 100).AsStream();
        }
    }
}
