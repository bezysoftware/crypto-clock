using CryptoClock.Configuration;
using CryptoClock.Widgets.Rendering.Nodes;
using SkiaSharp;

namespace CryptoClock.Widgets.Rendering
{
    public record RenderContext(
        SKSizeI AvailableSize,
        SKPaint Paint,
        ScreenConfig Screen,
        SKColor Background)
    {
        public int GetFontSize(FontSize fontSize)
        {
            var div = fontSize switch
            {
                FontSize.ExtraSmall => 34,
                FontSize.Small => 21,
                FontSize.Medium => 13,
                FontSize.Large => 8,
                FontSize.ExtraLarge => 5,
                FontSize.Huge => 3,
                _ => 13,
            };

            return this.Screen.Height / div;
        }
    }
}
