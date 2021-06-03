using System.IO;

namespace CryptoClock.Screens
{
    public interface IScreenPrinter
    {
        void Print(Stream stream);
    }
}
