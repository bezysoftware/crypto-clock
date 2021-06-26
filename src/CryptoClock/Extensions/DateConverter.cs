using Newtonsoft.Json.Converters;

namespace CryptoClock.Extensions
{
    public class DateConverter : IsoDateTimeConverter
    {
        public DateConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }
}
