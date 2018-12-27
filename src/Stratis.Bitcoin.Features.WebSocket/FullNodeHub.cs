using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System;
using Microsoft.Extensions.DependencyInjection;
using Stratis.Bitcoin.Features.RPC.Controllers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Dynamic;
using Stratis.Bitcoin.Features.Wallet.Models;
using System.Reflection;

namespace Stratis.Bitcoin.Features.WebSocket
{
    public class FullNodeHub : Hub
    {
        private readonly IWebSocketService webSocketService;
        private readonly FullNode fullNode;
        private readonly ControllerMethodList methods;
        private readonly IServiceProvider serviceProvider;

        public FullNodeHub(IWebSocketService webSocketService, FullNode fullNode, ControllerMethodList methods, IServiceProvider serviceProvider)
        {
            this.fullNode = fullNode;
            this.webSocketService = webSocketService;
            this.methods = methods;
            this.serviceProvider = serviceProvider;
        }

        public void Subscribe(string channel)
        {
            // TODO: Do something like this.
            //this.webSocketService.Subscribe(channel);
        }

        public async Task SendMessage(string user, string message)
        {
            var methodDescription = methods.List[34];
            Type type = methodDescription.Type;

            var controller = serviceProvider.GetService(type);

            var methodType = type.GetMethod(methodDescription.Action);

            var parameter = methodDescription.Parameters.FirstOrDefault();

            ConstructorInfo requestConstructor = parameter.ParameterType.GetConstructor(Type.EmptyTypes);
            dynamic request = requestConstructor.Invoke(new object[] { });
            request.WalletName = "default";
            request.AccountName = "account 0";

            dynamic result = methodType.Invoke(controller, new object[1] { request });

            if (result.Value != null)
            {
                result = result.Value;
            }

            var fullNodeController = serviceProvider.GetService<FullNodeController>();
            var rpcController = serviceProvider.GetService<RPCController>();

            // var results = rpcController.CallByName(JObject.Parse("{'jsonrpc': '1.0', 'id': 'curltest', 'method': 'getnewaddress', 'params': [''] }"));
            var getInfo = fullNodeController.GetInfo();

            await Clients.All.SendAsync("ReceiveMessage", user, JsonConvert.SerializeObject(getInfo));
            // await Clients.All.SendAsync("ReceiveMessage", user, JsonConvert.SerializeObject(result));
        }

        public void BroadcastMessage(string name, string message)
        {
            if (message == "stats")
            {
                Command(message);
                return;
            }

            this.Clients.All.SendAsync("broadcastMessage", name, message);
        }

        public void Command(string commandName)
        {
            if (commandName == "stats")
            {
                this.Clients.Caller.SendAsync("broadcastMessage", commandName, this.fullNode.LastLogOutput);
            }
        }

        public void Echo(string name, string message)
        {
            this.Clients.Client(this.Context.ConnectionId).SendAsync("echo", name, message + " (echo from server)");
        }
    }
}
