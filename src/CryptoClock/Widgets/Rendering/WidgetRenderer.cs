using CryptoClock.Configuration;
using CryptoClock.Widgets.Rendering.Nodes;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CryptoClock.Widgets.Rendering
{
    public class WidgetRenderer : WidgetRendererBase, IWidgetRenderer<RenderContext, RenderedResult>
    {
        private readonly IMemoryCache cache;
        private readonly ILogger<WidgetRenderer> log;

        public WidgetRenderer(
            IOptions<RenderConfig> options, 
            IOptions<ScreenConfig> screen,
            IMemoryCache cache,
            ILogger<WidgetRenderer> log) : base(options, screen)
        {
            this.cache = cache;
            this.log = log;
        }

        public override Stream Render(Widget widget, int width, int height, int columns, int rows)
        {
            var context = new RenderContext(
                new SKSizeI(width, height),
                new SKPaint
                {
                    IsAntialias = false,
                    SubpixelText = false,
                    Typeface = LoadTypeface(this.renderConfig.DefaultFont, SKFontStyleWeight.Normal),
                    Color = SKColor.Parse(widget.Config.Foreground)
                },
                this.screenConfig,
                SKColor.Parse(widget.Config.Background)
            );

            var size = $"{columns}x{rows}";
            var b = widget.Node.Bindings.FirstOrDefault(x => x.Sizes.Split(',', StringSplitOptions.RemoveEmptyEntries).Any(s => s == size))
                    ?? widget.Node.Bindings.First(x => x.Sizes == "*");

            return b.Render(context, this).Image.Encode(SKEncodedImageFormat.Png, 100).AsStream();
        }

        public RenderedResult Render(RenderContext context, BindingNode binding)
        {
            var edge = this.renderConfig.Margin * 2;
            var info = new SKImageInfo(context.AvailableSize.Width, context.AvailableSize.Height);
            using var surface = SKSurface.Create(info);

            surface.Canvas.Clear(context.Background);

            ApplyFontPaint(binding, context, context.Paint);

            var ctx = context with
            {
                AvailableSize = new SKSizeI(info.Size.Width - edge, info.Size.Height - edge)
            };
            var result = Render(ctx, binding, binding.Justify);

            surface.Canvas.DrawImage(result.Image, new SKPoint(this.renderConfig.Margin, this.renderConfig.Margin));

            return new RenderedResult(surface.Snapshot(), context.AvailableSize);
        }

        public RenderedResult Render(RenderContext context, RowNode row)
        {
            var widthSum = row.Columns.Sum(x => x.Width.Size);
            var results = new RenderedResult[row.Columns.Count];
            var columns = row.Columns.Select((x, i) => (column: x, i)).ToArray();
            var availableWidth = context.AvailableSize.Width;

            // first draw auto columns, then the rest with remaining size
            foreach (var (column, i) in columns.Where(x => x.column.Width.Auto))
            {
                var ctx = context with
                {
                    AvailableSize = new SKSizeI(0, context.AvailableSize.Height)
                };

                var result = column.Render(ctx, this);

                results[i] = result;
                availableWidth -= result.UsedSize.Width;
            }

            // then draw other proportionate columns
            foreach (var (column, i) in columns.Where(x => !x.column.Width.Auto))
            {
                var width = (int)((float)availableWidth / widthSum * column.Width.Size);
                var ctx = context with
                {
                    AvailableSize = new SKSizeI(width, context.AvailableSize.Height)
                };

                var result = column.Render(ctx, this);
                results[i] = result with { UsedSize = new SKSizeI(width, result.UsedSize.Height)  };
            }

            var maxHeight = results.Max(x => x.UsedSize.Height);
            var info = new SKImageInfo(context.AvailableSize.Width, maxHeight);
            var left = 0;

            using var surface = SKSurface.Create(info);

            // merge everything
            foreach (var (result, i) in results.Select((x, i) => (x, i)))
            {
                var top = GetPosition(columns[i].column.Align, maxHeight, result.UsedSize.Height);
                surface.Canvas.DrawImage(result.Image, new SKPoint(left, top));
                left += result.UsedSize.Width;
            }

            return new RenderedResult(surface.Snapshot(), info.Size);
        }

        public RenderedResult Render(RenderContext context, ColumnNode column)
        {
            return Render(context, column, JustifyContent.Start);
        }

        public RenderedResult Render(RenderContext context, BlockNode block)
        {
            return Render(context, block, JustifyContent.Start);
        }

        public RenderedResult Render(RenderContext context, TextNode text)
        {
            var bounds = new SKRect();
            var paint = ApplyFontPaint(text, context, context.Paint.Clone());

            paint.MeasureText(text.Text.AsSpan(), ref bounds);

            var height = (int)bounds.Size.Height;
            var width = (int)Math.Ceiling(bounds.Right);
            var info = new SKImageInfo(width, height);
            
            using var surface = SKSurface.Create(info);
            using var font = new SKFont
            {
                Edging = SKFontEdging.Alias,
                Size = paint.TextSize,
                Typeface = paint.Typeface
            };
            
            surface.Canvas.DrawText(text.Text, 0, -bounds.Top, font, paint);
            
            return new RenderedResult(surface.Snapshot(), info.Size);
        }

        public RenderedResult Render(RenderContext context, ImageNode image)
        {
            using var bitmap = SKBitmap.Decode(image.Name);

            var coef = image.Spacing switch
            {
                Spacing.None => 1f,
                Spacing.Regular => 2f,
                _ => 2f
            };

            var width = context.AvailableSize.Width / coef;
            var scale = width / bitmap.Width;
            var info = new SKImageInfo(context.AvailableSize.Width, (int)(bitmap.Height * scale));
            
            var paint = context.Paint.Clone();

            if (paint.Color == SKColors.White)
            {
                // images are black, if foreground should be white, use invert matrix
                var invertMatrix = new[]
                {
                    -1.0f,  0.0f,  0.0f,  1.0f,  0.0f,
                    0.0f,  -1.0f,  0.0f,  1.0f,  0.0f,
                    0.0f,  0.0f,  -1.0f,  1.0f,  0.0f,
                    1.0f,  1.0f,  1.0f,  1.0f,  0.0f
                };

                paint.ColorFilter = SKColorFilter.CreateColorMatrix(invertMatrix);
            }

            using var surface = SKSurface.Create(info);

            surface.Canvas.Scale(scale);
            surface.Canvas.DrawBitmap(bitmap, bitmap.Width - bitmap.Width / coef, 0, paint);

            return new RenderedResult(surface.Snapshot(), info.Size);
        }

        public RenderedResult Render(RenderContext context, LineNode line)
        {
            var paint = context.Paint.Clone();
            var size = context.GetFontSize(line.Size) / 5;
            var height = Math.Max(size, 1);
            var spacing = line.IsDivider ? context.GetFontSize(FontSize.Small) : 0;
            var info = new SKImageInfo(context.AvailableSize.Width, height + spacing * 2);
            
            using var surface = SKSurface.Create(info);

            if (!string.IsNullOrEmpty(line.Foreground))
            {
                paint.Color = SKColor.Parse(line.Foreground);
            }

            surface.Canvas.DrawRect(0, spacing, context.AvailableSize.Width, height, paint);

            return new RenderedResult(surface.Snapshot(), info.Size);
        }

        protected virtual RenderedResult Render(
            RenderContext context, 
            IVerticalElementsNode node,
            JustifyContent justify)
        {
            var heightLeft = context.AvailableSize.Height;
            var results = new List<RenderedResult>();

            foreach (var e in node.Elements)
            {
                var ctx = context with
                {
                    AvailableSize = new SKSizeI(context.AvailableSize.Width, heightLeft)
                };

                var result = e.Render(ctx, this);

                results.Add(result);
                heightLeft -= result.UsedSize.Height + this.renderConfig.Margin;
            }

            heightLeft += this.renderConfig.Margin; // last item doesn't consume margin

            var maxWidth = results.Max(x => (int?)x.UsedSize.Width) ?? context.AvailableSize.Width;
            var items = results.Count;
            var spaceY = justify == JustifyContent.Spread ? heightLeft / Math.Max(1f, items - 1) + this.renderConfig.Margin : this.renderConfig.Margin;
            var height = justify == JustifyContent.Start ? context.AvailableSize.Height - heightLeft : context.AvailableSize.Height;
            var offsetY = justify switch
            {
                JustifyContent.Center => heightLeft / 2,
                JustifyContent.End => heightLeft,
                JustifyContent.Spread 
                    when items == 1 => heightLeft / 2,
                _ => 0f
            };

            var info = new SKImageInfo(Math.Max(maxWidth, context.AvailableSize.Width), Math.Max(height, 1));
            using var surface = SKSurface.Create(info);

            foreach (var (result, i) in results.Select((x, i) => (x, i)))
            {
                var left = GetPosition(node.Elements[i].Align, info.Width, result.UsedSize.Width);
                var total = spaceY + result.UsedSize.Height;
                surface.Canvas.DrawImage(result.Image, new SKPoint(left, offsetY));
                offsetY += total;
                result.Image.Dispose();
            }

            return new RenderedResult(surface.Snapshot(), info.Size);
        }

        protected virtual SKPaint ApplyFontPaint(IFontNode node, RenderContext context, SKPaint paint)
        {
            var fontSize = node.FontSize != FontSize.Default ? node.FontSize : FontSize.Default;
            paint.TextSize = context.GetFontSize(fontSize);

            if (!string.IsNullOrWhiteSpace(node.Font) || !string.IsNullOrWhiteSpace(node.FontWeight))
            {
                var font = !string.IsNullOrWhiteSpace(node.Font) ? node.Font : paint.Typeface.FamilyName;
                var weight = !string.IsNullOrWhiteSpace(node.FontWeight)
                    ? Enum.Parse<SKFontStyleWeight>(node.FontWeight, true)
                    : (SKFontStyleWeight)paint.Typeface.FontWeight;

                paint.Typeface = LoadTypeface(font, weight);
            }

            return paint;
        }

        private SKTypeface LoadTypeface(string name, SKFontStyleWeight weight)
        {
            return this.cache.GetOrCreate((name, weight), x => FontLoader.LoadTypeface(name, weight));
        }

        private static int GetPosition(Align align, int availableSize, int usedSize)
        {
            return align switch
            {
                Align.Center => availableSize / 2 - usedSize / 2,
                Align.End => availableSize - usedSize,
                _ => 0,
            };
        }
    }
}
