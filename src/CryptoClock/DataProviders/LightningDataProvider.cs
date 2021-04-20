using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CryptoClock.Configuration;
using CryptoClock.Models;
using CryptoClock.Services.Lnd;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;

using static CryptoClock.Services.Lnd.Lightning;

namespace CryptoClock.DataProviders
{
    public class LightningDataProvider : IDataProvider
    {
        private const string ReadOnlyMacaroon = "readonly.macaroon";
        private const string Certificate = "tls.cert";
        private readonly LightningClient client;

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
        }

        public async Task<CryptoModel> EnrichAsync(CryptoModel model)
        {         
            var channels = await client.ListChannelsAsync(new ListChannelsRequest());
            var balance = await client.WalletBalanceAsync(new WalletBalanceRequest());

            var local = channels.Channels.Sum(x => x.LocalBalance);
            var remote = channels.Channels.Sum(x => x.RemoteBalance);

            return model with 
            {
                Lightning = new LightningModel(balance.ConfirmedBalance, balance.UnconfirmedBalance, local, remote)
            };
        }
    }
}
