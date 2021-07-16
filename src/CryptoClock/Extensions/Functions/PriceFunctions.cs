using Scriban.Runtime;
using System.Globalization;
using System.Linq;

namespace CryptoClock.Extensions.Functions
{
    public class PriceFunctions : ScriptObject
    {
        public static readonly string MemberName = "price";

        private static readonly CultureInfo[] cultures;

        static PriceFunctions()
        {
            cultures = CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .ToArray();
        }

        public static string Format(decimal price, string symbol)
        {
            var decimals = price < 1000 ? 2 : 0;
            var culture = cultures.FirstOrDefault(x => x.NumberFormat.CurrencySymbol == symbol);

            return string.Format(culture, $"{{0:C{decimals}}}", price);
        }
    }
}
