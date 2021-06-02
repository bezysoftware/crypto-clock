﻿using System.Collections.Generic;

namespace CryptoClock.Widgets.Repository
{
    public class WidgetRepository : IWidgetRepository
    {
        public IEnumerable<Widget> GetActiveWidgets()
        {
            return new[]
            {
                new Widget
                {
                    Id = "Weather",
                    Left = 0,
                    Top = 0,
                    Cols = 4,
                    Rows = 4
                },
                new Widget
                {
                    Id = "Lightning",
                    Left = 4,
                    Top = 2,
                    Cols = 6,
                    Rows = 4
                }
            };
        }

        public IEnumerable<WidgetPreview> GetAvailableWidgets()
        {
            return new[]
            {
                new WidgetPreview("Weather", "Description"),
                new WidgetPreview("Lightning", "Description")
            };
        }
    }
}
