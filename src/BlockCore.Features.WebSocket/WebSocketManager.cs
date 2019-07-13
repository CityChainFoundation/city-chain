using System;
using System.Collections.Generic;
using System.Text;
using Stratis.Bitcoin.EventBus;
using Stratis.Bitcoin.EventBus.CoreEvents;
using Stratis.Bitcoin.Signals;
using NBitcoin;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace BlockCore.Features.WebSocket
{
    public interface IWebSocketManager
    {

    }

    public class WebSocketManager : IWebSocketManager
    {
        private SubscriptionToken transactionReceivedSubscription;
        private SubscriptionToken blockConnectedSubscription;
        private readonly ISignals signals;
        private IHubContext<NodeHub> context;
        private readonly INodeContextConnector connector;
        private readonly Network network;

        public WebSocketManager(INodeContextConnector connector, ISignals signals, Network network)
        {
            this.connector = connector;
            this.signals = signals;
            this.network = network;
            this.transactionReceivedSubscription = this.signals.Subscribe<TransactionReceived>(this.ProcessTransaction);
            this.blockConnectedSubscription = this.signals.Subscribe<BlockConnected>(this.OnBlockConnected);
        }

        private void Initialize()
        {
            if (this.context == null)
            {
                this.context = this.connector.Context;
            }
        }

        public void ProcessTransaction(TransactionReceived transactionReceived)
        {
            Initialize();

            this.context.Clients.Group(nameof(TransactionReceived)).SendAsync(nameof(TransactionReceived), transactionReceived.ReceivedTransaction.ToHex(this.network));
        }

        private void OnBlockConnected(BlockConnected blockConnected)
        {
            Initialize();

            this.context.Clients.Group(nameof(BlockConnected)).SendAsync(nameof(BlockConnected), blockConnected.ConnectedBlock.Block.ToHex(this.network));
        }

        public void Start()
        { }

        public void Stop()
        {
            this.signals.Unsubscribe(this.transactionReceivedSubscription);
            this.signals.Unsubscribe(this.blockConnectedSubscription);
        }
    }
}
