using System;

namespace CryptoClock.Data.Bitcoin
{
    public record ExtendedTransaction(
        double Fee,
        double Size,
        double VSize,
        double Weight
    )
    {
        public double FeePerVsize => Math.Max(1, Fee / (Weight / 4));
    }
}