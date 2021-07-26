using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CryptoClock.Configuration;
using CryptoClock.Data.Models;
using CryptoClock.Extensions;
using CryptoClock.Data.Lnd;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;

using static CryptoClock.Data.Lnd.Lightning;
using System.Collections.Generic;

namespace CryptoClock.Data
{
    public class LightningDataProvider : IDataProvider
    {
        private const string ReadOnlyMacaroon = "readonly.macaroon";
        private const string Certificate = "tls.cert";
        private readonly LightningClient client;
        private readonly Dictionary<string, NodeInfo> nodes;

        public LightningDataProvider(IOptions<LightningConfig> options)
        {
            var macaroonBytes = File.ReadAllBytes(Path.Combine(options.Value.MacaroonPath, ReadOnlyMacaroon));
            var macaroon = BitConverter.ToString(macaroonBytes).Replace("-", "");
            var http = new HttpClientWithCertificate(Path.Combine(options.Value.CertificatePath, Certificate));

            var callCredentials = CallCredentials.FromInterceptor((context, metadata) => {
                metadata.Add("macaroon", macaroon);    
                return Task.CompletedTask;
            });

            var combined = ChannelCredentials.Create(new SslCredentials(), callCredentials);
            var channel = GrpcChannel.ForAddress(options.Value.ServiceUrl, new GrpcChannelOptions 
            {
                Credentials = combined,
                HttpClient = http
            });

            this.client = new LightningClient(channel);
            this.nodes = new();
        }

        public async Task<CryptoModel> EnrichAsync(CryptoModel model)
        {         
            var channels = await client.ListChannelsAsync(new ListChannelsRequest());
            var balance = await client.WalletBalanceAsync(new WalletBalanceRequest());

            await PopulateNodesCacheAsync(channels.Channels);

            Func<Channel, decimal, decimal> initiatorBalance = (channel, balance) => balance + (channel.Initiator ? channel.CommitFee : 0);
            var chs = channels.Channels
                .Select(x => new LightningChannelModel(
                    this.nodes.GetValueOrDefault(x.RemotePubkey)?.Node.Alias ?? x.RemotePubkey,
                    initiatorBalance(x, x.LocalBalance) / Consts.SatoshisInBitcoinD,
                    initiatorBalance(x, x.RemoteBalance) / Consts.SatoshisInBitcoinD))
                .ToArray();

            var confirmed = balance.ConfirmedBalance / Consts.SatoshisInBitcoinD;
            var unconfirmed = balance.UnconfirmedBalance / Consts.SatoshisInBitcoinD;

            return model with 
            {
                Lightning = new LightningModel(
                    new LightningWallet(confirmed, unconfirmed),
                    chs)
            };
        }

        private async Task PopulateNodesCacheAsync(IEnumerable<Channel> channels)
        {
            foreach (var channel in channels)
            {
                if (!this.nodes.ContainsKey(channel.RemotePubkey))
                {
                    var node = await this.client.GetNodeInfoAsync(new NodeInfoRequest { PubKey = channel.RemotePubkey });
                    this.nodes[channel.RemotePubkey] = node;
                }
            }
        }
    }
}
