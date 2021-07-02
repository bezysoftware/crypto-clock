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
        public int MinutesAgo => (int)(DateTime.UtcNow - Timestamp).TotalMinutes;
        
        public string FormattedSize => ByteSize.FromBytes(Size).ToString();
    }

}
