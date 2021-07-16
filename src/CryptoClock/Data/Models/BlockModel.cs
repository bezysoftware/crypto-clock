using ByteSizeLib;
using System;

namespace CryptoClock.Data.Models
{
    public record BlockModel(
        int Height, 
        DateTime Timestamp,
        int Transactions,
        double Size,
        double AvgFeeRate,
        double MinFeeRate,
        double MaxFeeRate)
    {
        public string FormattedTimestamp => Height == 0 
            ? "In ≈10 minutes"
            : $"{(int)(DateTime.UtcNow - Timestamp).TotalMinutes} minutes ago";
        
        public string FormattedSize => ByteSize.FromBytes(Size).ToString();

        public string DisplayHeight => Height == 0 ? "Next" : Height.ToString();
    }
}
