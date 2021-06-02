﻿namespace CryptoClock.Widgets.Rendering.Nodes
{
    public interface IFontNode
    {
        string Font { get; }
        
        string FontWeight { get; }

        FontSize FontSize { get; }
    }
}
