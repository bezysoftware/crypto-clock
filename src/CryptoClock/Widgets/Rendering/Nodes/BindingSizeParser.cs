using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CryptoClock.Widgets.Rendering.Nodes
{
    public static class BindingSizeParser
    {
        public static IEnumerable<BindingSize> Parse(string sizes)
        {
            foreach (var size in sizes.Split(",", StringSplitOptions.RemoveEmptyEntries))
            {
                var match = Regex.Match(size, "^([0-9]+[+]?|[*])x([0-9]+[+]?|[*])$");
                if (!match.Success)
                {
                    throw new InvalidOperationException($"Specified input '{size}' is not a valid size");
                }

                var (cols, moreCols) = ParseDimension(match.Groups[1].Value);
                var (rows, moreRows) = ParseDimension(match.Groups[2].Value);

                yield return new BindingSize(cols, moreCols, rows, moreRows);
            }
        }

        private static (int size, bool more) ParseDimension(string dimension)
        {
            if (dimension == "*")
            {
                return (1, true);
            }

            var size = int.Parse(dimension.TrimEnd('+'));

            return (size, dimension.EndsWith('+'));
        }
    }
}
