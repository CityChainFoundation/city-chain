using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NBitcoin;
using Stratis.Bitcoin.Builder;
using Stratis.Bitcoin.Builder.Feature;
using Stratis.Bitcoin.Configuration;
using Stratis.Bitcoin.Configuration.Settings;
using Stratis.Bitcoin.Features.Wallet;

namespace Stratis.Bitcoin.Features.WalletNotify
{
    public class WalletNotifyFeature : FullNodeFeature
    {
        public WalletNotifyFeature(
            //ConnectionManagerSettings connectionManagerSettings,
            //Network network,
            //NodeSettings nodeSettings,
            BaseWalletFeature walletFeature,
            ILoggerFactory loggerFactory)
        {

        }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }



    /// <summary>
    /// A class providing extension methods for <see cref="IFullNodeBuilder"/>.
    /// </summary>
    public static class FullNodeBuilderWalletNotifyExtension
    {
        public static IFullNodeBuilder UseWalletNotify(this IFullNodeBuilder fullNodeBuilder)
        {
            fullNodeBuilder.ConfigureFeature(features =>
            {
                features
                .AddFeature<WalletNotifyFeature>()
                .DependOn<BaseWalletFeature>()
                .FeatureServices(services =>
                {
                    // services.AddSingleton<IBlockNotification, BlockNotification>();
                    // services.AddSingleton<NotificationsController>();
                });
            });

            return fullNodeBuilder;
        }
    }
}
