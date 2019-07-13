using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NBitcoin;
using Stratis.Bitcoin;
using Stratis.Bitcoin.Builder;
using Stratis.Bitcoin.Builder.Feature;

namespace BlockCore.Features.WebSocket
{
    /// <summary>
    /// Provides an Api to the full node
    /// </summary>
    public sealed class WebSocketFeature : FullNodeFeature
    {
        /// <summary>How long we are willing to wait for the web socket to stop.</summary>
        private const int WebSocketStopTimeoutSeconds = 10;

        private readonly IFullNodeBuilder fullNodeBuilder;

        private readonly FullNode fullNode;

        private readonly WebSocketSettings settings;

        private readonly WebSocketFeatureOptions apiFeatureOptions;

        private readonly ILogger logger;

        private IWebHost webHost;

        public WebSocketFeature(
            IFullNodeBuilder fullNodeBuilder,
            FullNode fullNode,
            WebSocketFeatureOptions apiFeatureOptions,
            WebSocketSettings settings,
            ILoggerFactory loggerFactory
            )
        {
            this.fullNodeBuilder = fullNodeBuilder;
            this.fullNode = fullNode;
            this.apiFeatureOptions = apiFeatureOptions;
            this.settings = settings;
            //this.certificateStore = certificateStore;
            this.logger = loggerFactory.CreateLogger(this.GetType().FullName);

            this.InitializeBeforeBase = true;
        }

        public override Task InitializeAsync()
        {
            this.logger.LogInformation("Web Socket starting on URL '{0}:{1}'.", "http://localhost", this.settings.WsPort);
            this.webHost = Program.Initialize(new string[] { }, this.fullNodeBuilder.Services, this.settings, this.fullNode);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Prints command-line help.
        /// </summary>
        /// <param name="network">The network to extract values from.</param>
        public static void PrintHelp(Network network)
        {
            WebSocketSettings.PrintHelp(network);
        }

        /// <summary>
        /// Get the default configuration.
        /// </summary>
        /// <param name="builder">The string builder to add the settings to.</param>
        /// <param name="network">The network to base the defaults off.</param>
        public static void BuildDefaultConfigurationFile(StringBuilder builder, Network network)
        {
            WebSocketSettings.BuildDefaultConfigurationFile(builder, network);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            // Make sure we are releasing the listening ip address / port.
            if (this.webHost != null)
            {
                this.logger.LogInformation("Web Socket stopping on URL '{0}:{1}'.", "http://localhost", this.settings.WsPort);
                this.webHost.StopAsync(TimeSpan.FromSeconds(WebSocketStopTimeoutSeconds)).Wait();
                this.webHost = null;
            }
        }
    }

    public sealed class WebSocketFeatureOptions
    {
    }

    /// <summary>
    /// A class providing extension methods for <see cref="IFullNodeBuilder"/>.
    /// </summary>
    public static class WebSocketFeatureExtension
    {
        public static IFullNodeBuilder UseWebSocket(this IFullNodeBuilder fullNodeBuilder, Action<WebSocketFeatureOptions> optionsAction = null)
        {
            // TODO: move the options in to the feature builder
            var options = new WebSocketFeatureOptions();
            optionsAction?.Invoke(options);

            fullNodeBuilder.ConfigureFeature(features =>
            {
                features
                .AddFeature<WebSocketFeature>()
                //.DependOn<TransactionNotificationFeature>()
                .FeatureServices(services =>
                {
                    services.AddSingleton(fullNodeBuilder);
                    services.AddSingleton(options);
                    services.AddSingleton<WebSocketSettings>();
                    services.AddSingleton<IWebSocketManager, WebSocketManager>();
                    services.AddSingleton<INodeContextConnector, NodeContextConnector>();
                });
            });

            return fullNodeBuilder;
        }
    }
}
