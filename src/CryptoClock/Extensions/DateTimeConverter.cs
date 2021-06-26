using Newtonsoft.Json.Converters;

namespace CryptoClock.Extensions
{
    public class DateTimeConverter : IsoDateTimeConverter
    {
        public DateTimeConverter()
        {
            DateTimeFormat = "yyyy-MM-dd HH:mm";
        }
    }
}
