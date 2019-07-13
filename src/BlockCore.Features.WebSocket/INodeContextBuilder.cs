using Microsoft.AspNetCore.SignalR;

namespace BlockCore.Features.WebSocket
{
    public interface INodeContextConnector
    {
        void Connect(IHubContext<NodeHub> context);

        IHubContext<NodeHub> Context { get; }
    }

    public class NodeContextConnector : INodeContextConnector
    {
        public NodeContextConnector()
        {
            
        }

        public void Connect(IHubContext<NodeHub> context)
        {
            this.Context = context;
        }

        public IHubContext<NodeHub> Context { get; private set; }
    }
}
