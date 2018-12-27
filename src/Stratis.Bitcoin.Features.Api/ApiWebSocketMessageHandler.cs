using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Stratis.Bitcoin.Features.Api
{
    public class ApiWebSocketMessageHandler : IApiWebSocketMessageHandler
    {
        public async Task SendInitialMessages(ApiWebSocket userWebSocket)
        {
            WebSocket webSocket = userWebSocket.WebSocket;

            var msg = new ApiWebSocketMessage
            {
                MessagDateTime = DateTime.Now,
                Type = WSMessageType.anyType,
                Text = "",
                Username = "system"
            };

            string serialisedMessage = JsonConvert.SerializeObject(msg);
            byte[] bytes = Encoding.ASCII.GetBytes(serialisedMessage);
            await webSocket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task HandleMessage(WebSocketReceiveResult result, byte[] buffer, ApiWebSocket userWebSocket, IApiWebSocketFactory wsFactory)
        {
            string msg = Encoding.ASCII.GetString(buffer);
            try
            {
                var message = JsonConvert.DeserializeObject<ApiWebSocketMessage>(msg);
                if (message.Type == WSMessageType.anyType)
                {
                    await BroadcastOthers(buffer, userWebSocket, wsFactory);
                }
            }
            catch (Exception e)
            {
                await userWebSocket.WebSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
            }
        }

        public async Task BroadcastOthers(byte[] buffer, ApiWebSocket userWebSocket, IApiWebSocketFactory wsFactory)
        {
            var others = wsFactory.Others(userWebSocket);
            foreach (var uws in others)
            {
                await uws.WebSocket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async Task BroadcastAll(byte[] buffer, ApiWebSocket userWebSocket, IApiWebSocketFactory wsFactory)
        {
            var all = wsFactory.All();
            foreach (var uws in all)
            {
                await uws.WebSocket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
