using System.Diagnostics;
using System.IO;

namespace CryptoClock.Screens
{
    public class DebugScreenPrinter : IScreenPrinter
    {
        public void Print(Stream stream)
        {
            var path = Path.GetFullPath("img.png");

            using (var file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                stream.CopyTo(file);
            }

            Process.Start(new ProcessStartInfo { FileName = path, Verb = "open", UseShellExecute = true });
        }
    }
}
