using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace BlockCore.Features.WebSocket
{
    public class SubscriptionEvent
    {
        public string[] Events { get; set; }
    }

    public class NodeHub : Hub
    {
        private readonly IWebSocketManager manager;

        public NodeHub(IWebSocketManager manager)
        {
            this.manager = manager;
        }

        public async Task Subscribe(SubscriptionEvent subscriptionEvent)
        {
            foreach (var eventName in subscriptionEvent.Events)
            {
                await this.Groups.AddToGroupAsync(this.Context.ConnectionId, eventName);
            }
        }

        public async Task Unsubscribe(SubscriptionEvent subscriptionEvent)
        {
            foreach (var eventName in subscriptionEvent.Events)
            {
                await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, eventName);
            }
        }
    }
}
