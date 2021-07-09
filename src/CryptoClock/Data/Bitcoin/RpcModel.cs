namespace CryptoClock.Data.Bitcoin
{
    internal record RpcResult<T>(T Result)
    { }

    internal record Block(
        int Height,
        int Time,
        int NTx,
        string PreviousBlockHash,
        double Size)
    { }

    internal record BlockStats(
        double AvgFeeRate,
        double MaxFeeRate,
        double MinFeeRate)
    { }

    internal record Transaction()
    { }

}
