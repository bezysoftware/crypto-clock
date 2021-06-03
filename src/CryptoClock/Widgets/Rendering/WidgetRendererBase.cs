using CryptoClock.Configuration;
using CryptoClock.Widgets.Rendering.Nodes;
using Microsoft.Extensions.Options;
using SkiaSharp;
using System.Collections.Generic;
using System.IO;

namespace CryptoClock.Widgets.Rendering
{
    public abstract class WidgetRendererBase : IWidgetRenderer
    {
        protected readonly ScreenConfig screen;

        public WidgetRendererBase(IOptions<ScreenConfig> options)
        {
            this.screen = options.Value;
        }

        public Stream Render(IEnumerable<Widget> widgets)
        {
            var w = this.screen.Width / this.screen.Cols;
            var h = this.screen.Height / this.screen.Rows;
            var surface = SKSurface.Create(new SKImageInfo(screen.Width, screen.Height));

            foreach (var widget in widgets)
            {
                using (var result = Render(widget.Node, widget.Placement.Cols * w, widget.Placement.Rows * h, widget.Placement.Cols, widget.Placement.Rows))
                {
                    surface.Canvas.DrawImage(SKImage.FromEncodedData(result), new SKPoint(widget.Placement.Left * w, widget.Placement.Top * h));
                }
            }

            return surface.Snapshot().Encode(SKEncodedImageFormat.Png, 100).AsStream();
        }

        public abstract Stream Render(WidgetNode widget, int width, int height, int columns, int rows);
    }
}
