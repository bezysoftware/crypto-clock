using System;

namespace CryptoClock.Models
{
    public record LightningModel(long ConfirmedBalance, long UnconfirmedBalance, long LocalBalance, long RemoteBalance)
    {
        public long TotalBalance => ConfirmedBalance + UnconfirmedBalance;
    }
}
