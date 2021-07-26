using System.Collections.Generic;
using System.Linq;

namespace CryptoClock.Data.Models
{
    public record LightningModel(LightningWallet Wallet, IEnumerable<LightningChannelModel> Channels)
    {
        public LightningChannelModel Total => new LightningChannelModel(
            "Total",
            Channels.Sum(x => x.LocalBalance),
            Channels.Sum(x => x.RemoteBalance));

        public IEnumerable<LightningChannelModel> ChannelsWithTotal => Channels.Prepend(Total);
    }

    public record LightningChannelModel(string Name, decimal LocalBalance, decimal RemoteBalance)
    {
    }

    public record LightningWallet(decimal ConfirmedBalance, decimal UnconfirmedBalance)
    {
        public decimal TotalBalance => ConfirmedBalance + UnconfirmedBalance;
    }
}
