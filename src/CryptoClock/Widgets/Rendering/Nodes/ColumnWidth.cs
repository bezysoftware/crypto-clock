namespace CryptoClock.Widgets.Rendering.Nodes
{
    public class ColumnWidth
    {
        public string OriginalString { get; init; }

        public bool Auto { get; init; }

        public int Size { get; init; }

        public static ColumnWidth Parse(string value)
        {
            var auto = value?.ToLower() == "auto";
            var size = 0;

            if (!auto && !int.TryParse(value, out size))
            {
                size = 1;
            }

            return new ColumnWidth
            {
                OriginalString = value,
                Auto = auto,
                Size = size
            };
        }

        public static ColumnWidth Default { get; } = Parse("1");
    }
}