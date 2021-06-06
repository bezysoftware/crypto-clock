using CryptoClock.Configuration;
using CryptoClock.Widgets.Rendering.Nodes;
using System.Collections.Generic;
using System.IO;

namespace CryptoClock.Widgets.Rendering
{
    public interface IWidgetRenderer
    {
        Stream Render(IEnumerable<Widget> widgets);
        
        Stream Render(Widget widget, int width, int height, int columns, int rows);
    }

    public interface IWidgetRenderer<TContext, TResult> : IWidgetRenderer
    {
        TResult Render(TContext context, BindingNode binding);
        TResult Render(TContext context, TextNode text);
        TResult Render(TContext context, RowNode row);
        TResult Render(TContext context, ColumnNode column);
        TResult Render(TContext context, ImageNode image);
        TResult Render(TContext context, LineNode rect);
        TResult Render(TContext context, BlockNode blockNode);
    }
}
