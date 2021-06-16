using CryptoClock.Configuration;
using Microsoft.Extensions.Options;
using SkiaSharp;
using System.Collections.Generic;
using System.IO;

namespace CryptoClock.Widgets.Rendering
{
    public abstract class WidgetRendererBase : IWidgetRenderer
    {
        protected readonly ScreenConfig screenConfig;
        protected readonly RenderConfig renderConfig;

        public WidgetRendererBase(IOptions<RenderConfig> renderOptions, IOptions<ScreenConfig> screenOptions)
        {
            this.screenConfig = screenOptions.Value;
            this.renderConfig = renderOptions.Value;
        }

        public Stream Render(IEnumerable<Widget> widgets)
        {
            var w = this.screenConfig.Width / this.screenConfig.Cols;
            var h = this.screenConfig.Height / this.screenConfig.Rows;
            var surface = SKSurface.Create(new SKImageInfo(screenConfig.Width, screenConfig.Height));

            surface.Canvas.Clear(SKColor.Parse(this.renderConfig.Background));

            foreach (var widget in widgets)
            {
                using (var result = Render(widget, widget.Placement.Cols * w, widget.Placement.Rows * h, widget.Placement.Cols, widget.Placement.Rows))
                {
                    surface.Canvas.DrawImage(SKImage.FromEncodedData(result), new SKPoint(widget.Placement.Left * w, widget.Placement.Top * h));
                }
            }

            return surface.Snapshot().Encode(SKEncodedImageFormat.Png, 100).AsStream();
        }

        public abstract Stream Render(Widget widget, int width, int height, int columns, int rows);
    }
}
