using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using City.Features.WalletService.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Stratis.Bitcoin.Builder;
using Stratis.Bitcoin.Builder.Feature;
using Stratis.Bitcoin.Features.Notifications;

namespace City.Features.WalletService
{
    public class WalletServiceFeature : FullNodeFeature
    {
        public WalletServiceFeature()
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
    public static class FullNodeBuilderWalletServiceExtension
    {
        /// <summary>
        /// Adds a watch only wallet component to the node being initialized.
        /// </summary>
        /// <param name="fullNodeBuilder">The object used to build the current node.</param>
        /// <returns>The full node builder, enriched with the new component.</returns>
        public static IFullNodeBuilder UseWalletService(this IFullNodeBuilder fullNodeBuilder)
        {
            fullNodeBuilder.ConfigureFeature(features =>
            {
                features
                    .AddFeature<WalletServiceFeature>()
                    //.DependOn<BlockNotificationFeature>()
                    //.DependOn<TransactionNotificationFeature>()
                    .FeatureServices(services =>
                    {
                        services.AddSingleton<IWalletServiceManager, WalletServiceManager>();
                        services.AddSingleton<WalletServiceController>();
                    });
            });

            return fullNodeBuilder;
        }
    }
}
