namespace CryptoClock.Widgets.Rendering.Nodes
{
    public record BindingSize(int Columns, bool MoreColumns, int Rows, bool MoreRows)
    {
        public bool Matches(int columns, int rows)
        {
            return (columns == Columns || MoreColumns && columns > Columns) &&
                   (rows == Rows || MoreRows && rows > Rows);
        }
    }
}
