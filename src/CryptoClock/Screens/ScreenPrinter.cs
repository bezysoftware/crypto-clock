using CryptoClock.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Drawing;
using System.IO;
using Waveshare;
using Waveshare.Devices;
using Waveshare.Interfaces;

namespace CryptoClock.Screens
{
    public class ScreenPrinter : IScreenPrinter, IDisposable
    {
        private readonly IEPaperDisplay display;

        public ScreenPrinter(IOptions<ScreenConfig> options)
        {
            this.display = EPaperDisplay.Create(Enum.Parse<EPaperDisplayType>(options.Value.Type));
        }

        public void Print(Stream stream)
        {
            using var bitmap = new Bitmap(stream);

            this.display.Clear();
            this.display.WaitUntilReady();
            this.display.DisplayImage(bitmap);
        }

        public void Dispose()
        {
            this.display.Dispose();
        }
    }
}
