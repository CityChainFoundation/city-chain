using System;
using System.Text;
using System.Timers;
using Microsoft.Extensions.Logging;
using NBitcoin;
using Stratis.Bitcoin.Configuration;
using Stratis.Bitcoin.Utilities;

namespace BlockCore.Features.WebSocket
{
    /// <summary>
    /// Configuration related to the web socket interface.
    /// </summary>
    public class WebSocketSettings
    {
        /// <summary>Instance logger.</summary>
        private readonly ILogger logger;

        /// <summary>Port of node's API interface.</summary>
        public int WsPort { get; set; }

        /// <summary>URI to node's API interface.</summary>
        public Timer KeepaliveTimer { get; private set; }

        /// <summary>
        /// Initializes an instance of the object from the node configuration.
        /// </summary>
        /// <param name="nodeSettings">The node configuration.</param>
        public WebSocketSettings(NodeSettings nodeSettings)
        {
            Guard.NotNull(nodeSettings, nameof(nodeSettings));

            this.logger = nodeSettings.LoggerFactory.CreateLogger(typeof(WebSocketSettings).FullName);

            TextFileConfiguration config = nodeSettings.ConfigReader;

            // Find out which port should be used for the web socket. // nodeSettings.Network must be extended with additional default ws port.
            this.WsPort = config.GetOrDefault("wsport", 4336, this.logger);

            // Set the keepalive interval (set in seconds).
            int keepAlive = config.GetOrDefault("keepalive", 0, this.logger);
            if (keepAlive > 0)
            {
                this.KeepaliveTimer = new Timer
                {
                    AutoReset = false,
                    Interval = keepAlive * 1000
                };
            }
        }

        /// <summary>Prints the help information on how to configure the web socket settings to the logger.</summary>
        /// <param name="network">The network to use.</param>
        public static void PrintHelp(Network network)
        {
            var builder = new StringBuilder();

            builder.AppendLine($"-wsport=<0-65535>                 Port of node's web socket interface. Defaults to 4336.");

            NodeSettings.Default(network).Logger.LogInformation(builder.ToString());
        }

        /// <summary>
        /// Get the default configuration.
        /// </summary>
        /// <param name="builder">The string builder to add the settings to.</param>
        /// <param name="network">The network to base the defaults off.</param>
        public static void BuildDefaultConfigurationFile(StringBuilder builder, Network network)
        {
            builder.AppendLine("####Web Socket Settings####");
            builder.AppendLine($"#wsport=4336");
        }
    }
}
