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
        public int GetElementSize(ElementSize fontSize)
        {
            var div = fontSize switch
            {
                ElementSize.ExtraSmall => 34,
                ElementSize.Small => 21,
                ElementSize.Medium => 13,
                ElementSize.Large => 8,
                ElementSize.ExtraLarge => 5,
                ElementSize.Huge => 3,
                _ => 13,
            };

            return this.Screen.Height / div;
        }
    }
}
