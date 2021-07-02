using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CryptoClock.Data
{
    public static class CurrencyMap
    {
        private static readonly Dictionary<string, string> SymbolsByCode;

        static CurrencyMap()
        {
            SymbolsByCode = new Dictionary<string, string>();

            SymbolsByCode = CultureInfo
                            .GetCultures(CultureTypes.SpecificCultures)
                            .Select(x => new RegionInfo(x.LCID))
                            .DistinctBy(x => x.ISOCurrencySymbol)
                            .ToDictionary(x => x.ISOCurrencySymbol, x => x.CurrencySymbol);
        }

        public static string GetSymbol(string code) => SymbolsByCode[code.ToUpper()];
    }
}
